using bexio_lib.Interfaces;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BexioLibTest.Unit
{
    /// <summary>
    /// Records requests and answers with a canned response.
    /// </summary>
    public class FakeBexioApi : IBexioApi
    {
        public string API_URL { get; init; } = "https://api.bexio.com";
        public RestClient CLIENT { get; init; } = new RestClient("https://api.bexio.com/");

        public List<(RestRequest Request, Method Method)> Calls { get; } = new();
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Content { get; set; } = "{}";

        public (RestRequest Request, Method Method) Last => this.Calls.Last();

        public string Resource => this.Last.Request.Resource;
        public Method LastMethod => this.Last.Method;
        public string Body => this.Last.Request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody)?.Value as string;
        public string Query(string name) => this.Last.Request.Parameters.FirstOrDefault(p => p.Type == ParameterType.QueryString && p.Name == name)?.Value as string;

        public IRestResponse Execute(RestRequest request, Method method)
        {
            this.Calls.Add((request, method));
            return new RestResponse { StatusCode = this.StatusCode, Content = this.Content, Request = request };
        }

        public Task<IRestResponse> ExecuteAsync(RestRequest request, Method method, CancellationToken cancellationToken = default)
            => Task.FromResult(this.Execute(request, method));

        public IRestResponse Get(RestRequest request) => this.Execute(request, Method.GET);
        public IRestResponse Post(RestRequest request) => this.Execute(request, Method.POST);
        public IRestResponse Put(RestRequest request) => this.Execute(request, Method.PUT);
        public IRestResponse Patch(RestRequest request) => this.Execute(request, Method.PATCH);
        public IRestResponse Delete(RestRequest request) => this.Execute(request, Method.DELETE);
    }
}
