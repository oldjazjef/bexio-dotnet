using bexio_lib.Data;
using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using bexio_lib.Models;
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
        public void GetAll()
        {
            var exception = Record.Exception(() => this._orderEndpoint.GetAll());
            Assert.Null(exception);
        }
    }
}
