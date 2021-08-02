using bexio_lib.Interfaces;
using bexio_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiCountryEndpoint : BexioApiFullEndpoint<BexioCountry>, IBexioApiCountryEndpoint
    {
        public BexioApiCountryEndpoint(IBexioApi api): base(api, "country")
        {

        }
    }
}
