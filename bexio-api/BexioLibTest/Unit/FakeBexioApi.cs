using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BexioLibTest.Unit
{
    /// <summary>
    /// Records requests and answers with a canned response.
    /// </summary>
    public class FakeBexioApi : IBexioApi
    {
        public string API_URL { get; } = "https://api.bexio.com";

        public List<(BexioRequest Request, HttpMethod Method)> Calls { get; } = new();
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Content { get; set; } = "null";

        public (BexioRequest Request, HttpMethod Method) Last => this.Calls.Last();

        public string Resource => this.Last.Request.Resource;
        public HttpMethod LastMethod => this.Last.Method;
        public string Body => this.Last.Request.JsonBody;
        public string Query(string name) => this.Last.Request.Query.FirstOrDefault(p => p.Key == name).Value;

        public BexioResponse Send(BexioRequest request, HttpMethod method)
        {
            this.Calls.Add((request, method));
            return new BexioResponse { StatusCode = this.StatusCode, Content = this.Content };
        }

        public Task<BexioResponse> SendAsync(BexioRequest request, HttpMethod method, CancellationToken cancellationToken = default)
            => Task.FromResult(this.Send(request, method));
    }
}
