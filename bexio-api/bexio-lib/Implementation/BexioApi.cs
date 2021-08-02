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
        public string ENDPOINT { get; init; }
        public RestClient CLIENT { get; init; }

        public BexioApi(string apiEndpoint, string apiKey)
        {
            this.ENDPOINT = apiEndpoint;

            // setup endpoint
            this.CLIENT = new RestClient(this.ENDPOINT);
            this.CLIENT.Authenticator = new JwtAuthenticator(apiKey);
            this.CLIENT.AddDefaultHeader("Accept", "application/json");
        }
    }
}
