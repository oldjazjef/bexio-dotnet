using bexio_lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    public class BexioApiEndopints : IBexioApiEndpoint
    {
        public IBexioApi API { get; init; }

        public BexioApiEndopints(IBexioApi api)
        {
            this.API = api;
        }


        #region

        #endregion
    }
}
