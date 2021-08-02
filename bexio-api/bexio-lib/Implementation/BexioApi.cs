using bexio_lib.Interfaces;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    public class BexioApi : IBexioApi
    {
        public string API_URL { get; init; }
        public RestClient CLIENT { get; init; }

        public BexioApi() { }

        /// <summary>
        /// Creates api object containing RestClient setup using JWT auth method
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static BexioApi UseJwt(string apiUrl, string apiKey)
        {
            var client = new RestClient(apiUrl);
            client.Authenticator = new JwtAuthenticator(apiKey);
            client.AddDefaultHeader("Accept", "application/json");

            return new BexioApi
            {
                API_URL = apiUrl,
                CLIENT = client
            };
            
        }

        /// <summary>
        /// Wrapper for posting data to api
        /// This wrapper handles results and exceptions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IRestResponse Post(RestRequest request)
        {
            return this.CLIENT.Post(request);
        }

        /// <summary>
        /// Wrapper for posting data to api
        /// This wrapper handles results and exceptions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IRestResponse Get(RestRequest request)
        {
            return this.CLIENT.Get(request);
        }
    }
}
