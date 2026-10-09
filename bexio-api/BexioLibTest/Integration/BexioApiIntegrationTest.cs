using bexio_lib.Interfaces;
using bexio_lib.Interfaces.V3;
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

        private readonly IBexioApiOrderEndpoint _orders;
        private readonly IBexioApiContactEndpoint _contacts;
        private readonly IBexioApiCurrencyV3Endpoint _currencies;

        public BexioApiIntegrationTest(IBexioApiOrderEndpoint orders, IBexioApiContactEndpoint contacts, IBexioApiCurrencyV3Endpoint currencies)
        {
            this._orders = orders;
            this._contacts = contacts;
            this._currencies = currencies;
        }

        [Fact]
        public void V2_Orders_GetAll()
        {
            if (!HasKey) return;
            Assert.Null(Record.Exception(() => this._orders.GetAll()));
        }

        [Fact]
        public void V2_Contacts_GetAll()
        {
            if (!HasKey) return;
            Assert.Null(Record.Exception(() => this._contacts.GetAll()));
        }

        [Fact]
        public void V3_Currencies_GetAll()
        {
            if (!HasKey) return;
            Assert.Null(Record.Exception(() => this._currencies.GetAll()));
        }
    }
}
