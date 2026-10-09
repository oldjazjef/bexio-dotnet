using bexio_lib.Interfaces;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    public class BexioApi : IBexioApi
    {
        public const string DEFAULT_API_URL = "https://api.bexio.com";

        public string API_URL { get; init; }
        public RestClient CLIENT { get; init; }

        public BexioApi() { }

        /// <summary>
        /// Creates api object containing RestClient setup using JWT / personal access token / OAuth2 bearer auth.
        /// The api version is NOT part of the url, every endpoint knows its own version (2.0, 3.0, ...).
        /// For backwards compatibility a trailing version segment ("https://api.bexio.com/2.0") is stripped.
        /// </summary>
        /// <param name="apiUrl">Base url, defaults to https://api.bexio.com</param>
        /// <param name="apiKey">Bearer token</param>
        public static BexioApi UseJwt(string apiUrl, string apiKey)
        {
            var baseUrl = NormalizeBaseUrl(apiUrl);
            var client = new RestClient(baseUrl + "/");
            client.Authenticator = new JwtAuthenticator(apiKey);
            client.AddDefaultHeader("Accept", "application/json");

            return new BexioApi
            {
                API_URL = baseUrl,
                CLIENT = client
            };
        }

        /// <summary>
        /// Removes trailing slashes and a trailing version segment (e.g. /2.0, /3.0) from the url.
        /// </summary>
        public static string NormalizeBaseUrl(string apiUrl)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                return DEFAULT_API_URL;
            }
            var url = apiUrl.Trim().TrimEnd('/');
            url = Regex.Replace(url, @"/\d+\.\d+$", string.Empty);
            return url;
        }

        public IRestResponse Execute(RestRequest request, Method method)
            => this.CLIENT.Execute(request, method);

        public Task<IRestResponse> ExecuteAsync(RestRequest request, Method method, CancellationToken cancellationToken = default)
            => this.CLIENT.ExecuteAsync(request, method, cancellationToken);

        public IRestResponse Get(RestRequest request) => this.Execute(request, Method.GET);
        public IRestResponse Post(RestRequest request) => this.Execute(request, Method.POST);
        public IRestResponse Put(RestRequest request) => this.Execute(request, Method.PUT);
        public IRestResponse Patch(RestRequest request) => this.Execute(request, Method.PATCH);
        public IRestResponse Delete(RestRequest request) => this.Execute(request, Method.DELETE);
    }
}
