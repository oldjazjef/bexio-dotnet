using bexio_lib.Implementation;
using bexio_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BexioLibTest.Unit
{
    public class StubHandler : HttpMessageHandler
    {
        public Queue<Func<HttpRequestMessage, HttpResponseMessage>> Responses { get; } = new();
        public List<(HttpRequestMessage Request, string Body)> Requests { get; } = new();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = request.Content == null ? null : await request.Content.ReadAsStringAsync();
            this.Requests.Add((request, body));
            return this.Responses.Count > 0 ? this.Responses.Dequeue()(request) : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") };
        }

        public static HttpResponseMessage Json(HttpStatusCode code, string json) =>
            new HttpResponseMessage(code) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
    }

    public class TransportTests
    {
        private readonly StubHandler _handler = new StubHandler();

        private BexioClient Client(Action<BexioOptions> configure = null)
        {
            var options = new BexioOptions { AccessToken = "tok", RetryBaseDelay = TimeSpan.Zero };
            configure?.Invoke(options);
            return new BexioClient(new BexioApi(new HttpClient(this._handler), options));
        }

        [Fact]
        public async Task Sends_bearer_token_and_builds_url()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[]"));
            await this.Client().V2.Contacts.GetAllAsync(new BexioRequestFilter { limit = 5, order_by = new[] { "id_desc" } });

            var (request, _) = this._handler.Requests.Single();
            Assert.Equal("https://api.bexio.com/2.0/contact?limit=5&order_by=id_desc", request.RequestUri.ToString());
            Assert.Equal("Bearer", request.Headers.Authorization.Scheme);
            Assert.Equal("tok", request.Headers.Authorization.Parameter);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public async Task Token_provider_is_used_per_request()
        {
            var n = 0;
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[]"));
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[]"));
            var client = this.Client(o => o.AccessTokenProvider = _ => Task.FromResult("t" + (++n)));
            await client.V3.Currencies.GetAllAsync();
            await client.V3.Currencies.GetAllAsync();
            Assert.Equal(new[] { "t1", "t2" }, this._handler.Requests.Select(r => r.Request.Headers.Authorization.Parameter));
        }

        [Fact]
        public async Task Sends_json_body_on_create()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.Created, "{\"id\":9}"));
            var created = await this.Client().V2.Contacts.CreateAsync(new BexioContact { name_1 = "Muster" });
            Assert.Equal(9, created.id);
            var (request, body) = this._handler.Requests.Single();
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("application/json", request.Content.Headers.ContentType.MediaType);
            Assert.Contains("\"name_1\":\"Muster\"", body);
        }

        [Fact]
        public async Task Retries_on_429_and_then_succeeds()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json((HttpStatusCode)429, "{\"message\":\"slow down\"}"));
            this._handler.Responses.Enqueue(_ => StubHandler.Json((HttpStatusCode)429, "{\"message\":\"slow down\"}"));
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[]"));
            var result = await this.Client().V2.Contacts.GetAllAsync();
            Assert.Empty(result);
            Assert.Equal(3, this._handler.Requests.Count);
        }

        [Fact]
        public async Task Gives_up_after_max_retries_and_throws()
        {
            for (var i = 0; i < 10; i++) this._handler.Responses.Enqueue(_ => StubHandler.Json((HttpStatusCode)429, "{\"message\":\"slow down\"}"));
            var ex = await Assert.ThrowsAsync<BexioApiException>(() => this.Client(o => o.MaxRetries = 2).V2.Contacts.GetAllAsync());
            Assert.Equal((HttpStatusCode)429, ex.StatusCode);
            Assert.Equal(3, this._handler.Requests.Count);
        }

        [Fact]
        public async Task Does_not_retry_client_errors()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.UnprocessableEntity, "{\"message\":\"bad\"}"));
            await Assert.ThrowsAsync<BexioApiException>(() => this.Client().V2.Contacts.CreateAsync(new BexioContact()));
            Assert.Single(this._handler.Requests);
        }

        [Fact]
        public async Task Upload_is_multipart_and_download_returns_bytes()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[{\"id\":3,\"name\":\"a.txt\"}]"));
            var file = await this.Client().V3.Files.UploadAsync("a.txt", Encoding.UTF8.GetBytes("hello"));
            Assert.Equal(3, file.id);
            var (request, body) = this._handler.Requests.Single();
            Assert.StartsWith("multipart/form-data", request.Content.Headers.ContentType.MediaType);
            Assert.Contains("filename=a.txt", body);
            Assert.Contains("hello", body);

            this._handler.Responses.Enqueue(_ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(new byte[] { 1, 2, 3 }) });
            Assert.Equal(new byte[] { 1, 2, 3 }, await this.Client().V3.Files.DownloadAsync(3));
        }

        [Fact]
        public async Task Pages_through_all_entities()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[{\"id\":1},{\"id\":2}]"));
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[{\"id\":3}]"));
            var ids = new List<int?>();
            await foreach (var c in this.Client().V2.Contacts.GetAllPagesAsync(pageSize: 2)) ids.Add(c.id);
            Assert.Equal(new int?[] { 1, 2, 3 }, ids);
            Assert.EndsWith("limit=2&offset=2", this._handler.Requests[1].Request.RequestUri.Query);
        }

        [Fact]
        public void Factory_creates_independent_clients_per_token()
        {
            var factory = new BexioClientFactory();
            var a = factory.Create("token-a");
            var b = factory.Create("token-b");
            Assert.NotSame(a, b);
            Assert.Throws<ArgumentException>(() => factory.Create(" "));
        }
    }
}
