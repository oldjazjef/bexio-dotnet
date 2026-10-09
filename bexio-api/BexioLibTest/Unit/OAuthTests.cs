using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using bexio_lib.OAuth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BexioLibTest.Unit
{
    public class TestTime : TimeProvider
    {
        public DateTimeOffset Now { get; set; } = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        public override DateTimeOffset GetUtcNow() => this.Now;
    }

    public class OAuthTests
    {
        private readonly StubHandler _handler = new StubHandler();
        private readonly TestTime _time = new TestTime();

        private readonly BexioOAuthOptions _options = new BexioOAuthOptions
        {
            ClientId = "my-app",
            ClientSecret = "s3cret",
            RedirectUri = "https://app.test/callback",
            Scopes = new[] { "contact_show", "kb_invoice_edit" }
        };

        private BexioOAuthClient Oauth() => new BexioOAuthClient(new HttpClient(this._handler), this._options, this._time);

        private static string TokenJson(string access, string refresh = "r2", int expiresIn = 300)
            => $"{{\"access_token\":\"{access}\",\"refresh_token\":\"{refresh}\",\"expires_in\":{expiresIn},\"scope\":\"openid\"}}";

        [Fact]
        public void Authorization_url_contains_client_redirect_state_and_scopes()
        {
            var url = new Uri(this.Oauth().GetAuthorizationUrl("xyz"));
            Assert.Equal("https://auth.bexio.com/realms/bexio/protocol/openid-connect/auth", url.GetLeftPart(UriPartial.Path));
            var query = System.Web.HttpUtility.ParseQueryString(url.Query);
            Assert.Equal("my-app", query["client_id"]);
            Assert.Equal("https://app.test/callback", query["redirect_uri"]);
            Assert.Equal("code", query["response_type"]);
            Assert.Equal("xyz", query["state"]);
            Assert.Equal("openid offline_access contact_show kb_invoice_edit", query["scope"]);
        }

        [Fact]
        public async Task Exchange_code_posts_form_and_maps_token()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, TokenJson("a1", "r1", 600)));
            var token = await this.Oauth().ExchangeCodeAsync("the-code");

            var (request, body) = this._handler.Requests.Single();
            Assert.Equal("https://auth.bexio.com/realms/bexio/protocol/openid-connect/token", request.RequestUri.ToString());
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Contains("grant_type=authorization_code", body);
            Assert.Contains("code=the-code", body);
            Assert.Contains("client_id=my-app", body);
            Assert.Contains("client_secret=s3cret", body);
            Assert.Contains("redirect_uri=https%3A%2F%2Fapp.test%2Fcallback", body);
            Assert.Equal("a1", token.AccessToken);
            Assert.Equal("r1", token.RefreshToken);
            Assert.Equal(this._time.Now.AddSeconds(600), token.ExpiresAt);
        }

        [Fact]
        public async Task Failed_token_request_throws_with_status()
        {
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.BadRequest, "{\"error\":\"invalid_grant\"}"));
            var ex = await Assert.ThrowsAsync<BexioApiException>(() => this.Oauth().ExchangeCodeAsync("bad"));
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
            Assert.Contains("invalid_grant", ex.Message);
        }

        private BexioOAuthTokenProvider Provider(InMemoryBexioTokenStore store)
            => new BexioOAuthTokenProvider(this.Oauth(), store, this._options, this._time);

        [Fact]
        public async Task Valid_token_is_returned_without_refresh()
        {
            var store = new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "a1", RefreshToken = "r1", ExpiresAt = this._time.Now.AddMinutes(10) });
            Assert.Equal("a1", await this.Provider(store).GetAccessTokenAsync());
            Assert.Empty(this._handler.Requests);
        }

        [Fact]
        public async Task Expiring_token_is_refreshed_and_rotated_refresh_token_saved()
        {
            var store = new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "old", RefreshToken = "r1", ExpiresAt = this._time.Now.AddSeconds(30) });
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, TokenJson("new", "r2")));

            Assert.Equal("new", await this.Provider(store).GetAccessTokenAsync());

            var (_, body) = this._handler.Requests.Single();
            Assert.Contains("grant_type=refresh_token", body);
            Assert.Contains("refresh_token=r1", body);
            var saved = await store.GetAsync();
            Assert.Equal("new", saved.AccessToken);
            Assert.Equal("r2", saved.RefreshToken);
        }

        [Fact]
        public async Task Concurrent_callers_cause_a_single_refresh()
        {
            var store = new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "old", RefreshToken = "r1", ExpiresAt = this._time.Now.AddSeconds(-1) });
            this._handler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, TokenJson("new")));
            var provider = this.Provider(store);

            var tokens = await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => provider.GetAccessTokenAsync()));

            Assert.All(tokens, t => Assert.Equal("new", t));
            Assert.Single(this._handler.Requests);
        }

        [Fact]
        public async Task Missing_token_or_refresh_token_gives_clear_error()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => this.Provider(new InMemoryBexioTokenStore()).GetAccessTokenAsync());
            var expired = new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "a", ExpiresAt = this._time.Now.AddDays(-1) });
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => this.Provider(expired).GetAccessTokenAsync());
            Assert.Contains("offline_access", ex.Message);
        }

        [Fact]
        public async Task Api_client_uses_oauth_token_for_requests()
        {
            var store = new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "oauth-token", RefreshToken = "r", ExpiresAt = this._time.Now.AddHours(1) });
            var apiHandler = new StubHandler();
            apiHandler.Responses.Enqueue(_ => StubHandler.Json(HttpStatusCode.OK, "[]"));
            var options = new BexioOptions { AccessTokenProvider = this.Provider(store).GetAccessTokenAsync };
            var client = new BexioClient(new BexioApi(new HttpClient(apiHandler), options));

            await client.V2.Contacts.GetAllAsync();

            Assert.Equal("oauth-token", apiHandler.Requests.Single().Request.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task Di_wires_oauth_into_the_api_client()
        {
            var services = new ServiceCollection();
            services.AddBexio(_ => { });
            services.AddBexioOAuth(o => { o.ClientId = "id"; o.ClientSecret = "secret"; o.RedirectUri = "https://x"; });
            services.AddSingleton<IBexioTokenStore>(new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "from-store", ExpiresAt = DateTimeOffset.UtcNow.AddHours(1) }));
            var provider = services.BuildServiceProvider();

            var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<BexioOptions>>().Value;
            Assert.NotNull(options.AccessTokenProvider);
            Assert.Equal("from-store", await options.AccessTokenProvider(default));
            Assert.NotNull(provider.GetRequiredService<IBexioOAuthClient>());
        }

        [Fact]
        public async Task Factory_extension_creates_client_for_token_provider()
        {
            var store = new InMemoryBexioTokenStore(new BexioOAuthToken { AccessToken = "tenant-token", ExpiresAt = this._time.Now.AddHours(1) });
            var client = new BexioClientFactory().Create(this.Provider(store));
            Assert.NotNull(client.V2.Invoices);
            Assert.Equal("tenant-token", await this.Provider(store).GetAccessTokenAsync());
        }
    }
}
