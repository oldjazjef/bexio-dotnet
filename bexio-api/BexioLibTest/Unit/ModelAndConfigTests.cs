using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace BexioLibTest.Unit
{
    public class ModelAndConfigTests
    {
        [Fact]
        public void Invoice_json_is_deserialized()
        {
            const string json = @"{""id"":12,""document_nr"":""RE-0012"",""title"":""Test"",""contact_id"":3,""user_id"":1,
                ""total_gross"":""119.0000"",""total_net"":""100.0000"",""kb_item_status_id"":9,""mwst_is_net"":true,
                ""positions"":[{""id"":1,""type"":""KbPositionCustom"",""amount"":""2"",""unit_price"":""50.0"",""text"":""Work""}]}";
            var invoice = JsonConvert.DeserializeObject<BexioInvoice>(json);
            Assert.Equal(12, invoice.id);
            Assert.Equal("RE-0012", invoice.document_nr);
            Assert.Equal("119.0000", invoice.total_gross);
            Assert.Equal(9, invoice.kb_item_status_id);
            Assert.Single(invoice.positions);
        }

        [Fact]
        public void Timesheet_tracking_serializes_without_nulls()
        {
            var timesheet = new BexioTimesheet
            {
                user_id = 1,
                tracking = new BexioTimesheetTracking { type = "duration", date = "2024-01-02", duration = "01:30" }
            };
            var api = new FakeBexioApi { Content = "{}" };
            new bexio_lib.Implementation.Endpoints.BexioApiTimesheetEndpoint(api).Create(timesheet);
            Assert.Equal("{\"user_id\":1,\"tracking\":{\"type\":\"duration\",\"date\":\"2024-01-02\",\"duration\":\"01:30\"}}", api.Body);
        }

        [Fact]
        public void Exception_message_contains_validation_errors()
        {
            var ex = new BexioApiException(HttpStatusCode.UnprocessableEntity, "{\"message\":\"Validation failed\",\"errors\":[\"name_1 required\"]}");
            Assert.Contains("422", ex.Message);
            Assert.Contains("Validation failed", ex.Message);
            Assert.Contains("name_1 required", ex.Message);
        }

        [Fact]
        public void Options_are_bound_from_configuration_section()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Bexio:AccessToken"] = "abc",
                ["Bexio:BaseUrl"] = "https://example.test",
                ["Bexio:MaxRetries"] = "7"
            }).Build();

            var provider = new ServiceCollection().AddBexio(config.GetSection("Bexio")).BuildServiceProvider();
            var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<BexioOptions>>().Value;
            Assert.Equal("abc", options.AccessToken);
            Assert.Equal("https://example.test", options.BaseUrl);
            Assert.Equal(7, options.MaxRetries);
            Assert.Equal("https://example.test", provider.GetRequiredService<IBexioApi>().API_URL);
        }

        [Fact]
        public void Legacy_AddBexioJwt_still_works()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["bexioApiKey"] = "abc",
                ["bexioApiUrl"] = "https://api.bexio.com/2.0"
            }).Build();

            var provider = new ServiceCollection().AddBexioJwt(config).BuildServiceProvider();
            Assert.Equal("https://api.bexio.com", provider.GetRequiredService<IBexioApi>().API_URL);
            Assert.NotNull(provider.GetRequiredService<IBexioApiInvoiceEndpoint>());
        }

        [Fact]
        public void Factory_resolved_from_di_creates_clients()
        {
            var provider = new ServiceCollection().AddBexio(o => o.AccessToken = "x").BuildServiceProvider();
            var client = provider.GetRequiredService<IBexioClientFactory>().Create("other");
            Assert.NotNull(client.V2.Invoices);
        }
    }
}
