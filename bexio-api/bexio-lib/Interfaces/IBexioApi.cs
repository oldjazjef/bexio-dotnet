using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    public interface IBexioApi
    {
        string API_URL { get; init; }
        RestClient CLIENT { get; init; }

        IRestResponse Post(RestRequest request);

        IRestResponse Get(RestRequest request);
    }
}
