using bexio_lib.Interfaces;
using bexio_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiContactEndpoint : BexioApiFullEndpoint<BexioContact>, IBexioApiContactEndpoint
    {
        public BexioApiContactEndpoint(IBexioApi api): base(api, "contact")
        {

        }
    }
}
