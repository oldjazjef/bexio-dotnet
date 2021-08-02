using bexio_lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BexioLibTest.Integration.Endpoints
{
    public class BexioApiOrderEndpointTest
    {
        private readonly IBexioApiOrderEndpoint _orderEndpoint; 

        public BexioApiOrderEndpointTest(IBexioApiOrderEndpoint orderEndpoint)
        {
            this._orderEndpoint = orderEndpoint;
        }

        [Fact]
        public void TestEndpoint()
        {
            var allOrders = this._orderEndpoint.GetAll();
            Assert.True(allOrders != null);
        }
    }
}
