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
        public string API_URL { get; init; }
        public RestClient CLIENT { get; init; }
    }
}
