using bexio_lib.Implementation;
using System;
using Xunit;

namespace BexioLibTest.Integration
{
    /// <summary>
    /// Runs only if the environment variable / apisecrets.json value "bexioApiKey" is set,
    /// otherwise the tests pass without calling the api.
    /// </summary>
    public class BexioApiIntegrationTest
    {
        private static bool HasKey => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("bexioApiKey"));

        private readonly IBexioClient _bexio;

        public BexioApiIntegrationTest(IBexioClient bexio)
        {
            this._bexio = bexio;
        }

        [Fact]
        public void V2_Orders_GetAll()
        {
            if (!HasKey) return;
            Assert.Null(Record.Exception(() => this._bexio.V2.Orders.GetAll()));
        }

        [Fact]
        public void V2_Contacts_GetAll()
        {
            if (!HasKey) return;
            Assert.Null(Record.Exception(() => this._bexio.V2.Contacts.GetAll()));
        }

        [Fact]
        public void V3_Currencies_GetAll()
        {
            if (!HasKey) return;
            Assert.Null(Record.Exception(() => this._bexio.V3.Currencies.GetAll()));
        }
    }
}
