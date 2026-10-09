using bexio_lib.Implementation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    /// <summary>
    /// Transport layer: sends a request to bexio (auth, base url, retry) and returns the raw response.
    /// </summary>
    public interface IBexioApi
    {
        /// <summary>
        /// Base url of the api without version segment, e.g. https://api.bexio.com
        /// </summary>
        string API_URL { get; }

        BexioResponse Send(BexioRequest request, HttpMethod method);
        Task<BexioResponse> SendAsync(BexioRequest request, HttpMethod method, CancellationToken cancellationToken = default);
    }
}
