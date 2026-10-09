using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    public interface IBexioApi
    {
        /// <summary>
        /// Base url of the api without version segment, e.g. https://api.bexio.com
        /// </summary>
        string API_URL { get; init; }
        RestClient CLIENT { get; init; }

        IRestResponse Execute(RestRequest request, Method method);
        Task<IRestResponse> ExecuteAsync(RestRequest request, Method method, CancellationToken cancellationToken = default);

        IRestResponse Get(RestRequest request);
        IRestResponse Post(RestRequest request);
        IRestResponse Put(RestRequest request);
        IRestResponse Patch(RestRequest request);
        IRestResponse Delete(RestRequest request);
    }
}
