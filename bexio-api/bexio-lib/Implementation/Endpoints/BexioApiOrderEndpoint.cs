using bexio_lib.Interfaces;
using bexio_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiOrderEndpoint : BexioApiFullEndpoint<BexioOrder>, IBexioApiOrderEndpoint
    {
        public BexioApiOrderEndpoint(IBexioApi api): base(api, "kb_order")
        {

        }
    }
}
