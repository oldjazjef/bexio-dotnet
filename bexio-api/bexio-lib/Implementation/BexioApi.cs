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
    }
}
