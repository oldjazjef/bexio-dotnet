using bexio_lib.Data;
using bexio_lib.Implementation;
using bexio_lib.Implementation.Endpoints;
using bexio_lib.Implementation.Endpoints.V3;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BexioLibTest.Unit
{
    public class EndpointTests
    {
        private readonly FakeBexioApi _api = new FakeBexioApi();

        [Fact]
        public void V2_resource_contains_version()
        {
            this._api.Content = "[]";
            new BexioApiContactEndpoint(this._api).GetAll();
            Assert.Equal("2.0/contact", this._api.Resource);
            Assert.Equal(Method.GET, this._api.LastMethod);
        }

        [Fact]
        public void V3_resource_contains_version()
        {
            this._api.Content = "[]";
            new BexioApiCurrencyV3Endpoint(this._api).GetAll();
            Assert.Equal("3.0/currencies", this._api.Resource);

            this._api.Content = "{\"next_ref_nr\":\"MA-1\"}";
            Assert.Equal("MA-1", new BexioApiManualEntryEndpoint(this._api).GetNextReferenceNumber());
            Assert.Equal("3.0/accounting/manual_entries/next_ref_nr", this._api.Resource);
        }

        [Fact]
        public void GetAll_sets_query_parameters()
        {
            this._api.Content = "[]";
            new BexioApiContactEndpoint(this._api).GetAll(new BexioRequestFilter { limit = 10, offset = 20, order_by = new[] { "id_desc", "name" } });
            Assert.Equal("10", this._api.Query("limit"));
            Assert.Equal("20", this._api.Query("offset"));
            Assert.Equal("id_desc,name", this._api.Query("order_by"));
        }

        [Fact]
        public void Search_posts_filters_as_array()
        {
            this._api.Content = "[]";
            var filter = new BexioRequestFilter().Add(new BexioRequestFilterInstruction(BexioInvoiceFilterFields.kb_item_status_id, "9,16", BexioFilterCriteria.IN));
            new BexioApiInvoiceEndpoint(this._api).Search(filter);
            Assert.Equal("2.0/kb_invoice/search", this._api.Resource);
            Assert.Equal(Method.POST, this._api.LastMethod);
            Assert.Equal("[{\"field\":\"kb_item_status_id\",\"value\":\"9,16\",\"criteria\":\"in\"}]", this._api.Body);
        }

        [Fact]
        public void Search_without_filter_sends_empty_array()
        {
            this._api.Content = "[]";
            new BexioApiInvoiceEndpoint(this._api).Search();
            Assert.Equal("[]", this._api.Body);
        }

        [Fact]
        public void Create_omits_null_properties()
        {
            this._api.Content = "{\"id\":5}";
            var result = new BexioApiContactEndpoint(this._api).Create(new BexioContact { name_1 = "Muster", contact_type_id = 1 });
            Assert.Equal(5, result.id);
            Assert.Equal(Method.POST, this._api.LastMethod);
            Assert.DoesNotContain("null", this._api.Body);
            Assert.Contains("\"name_1\":\"Muster\"", this._api.Body);
        }

        [Fact]
        public void Update_uses_post_in_v2_and_put_in_v3()
        {
            new BexioApiContactEndpoint(this._api).Update(7, new BexioContact { name_1 = "x" });
            Assert.Equal("2.0/contact/7", this._api.Resource);
            Assert.Equal(Method.POST, this._api.LastMethod);

            new BexioApiCurrencyV3Endpoint(this._api).Update(7, new BexioCurrency { name = "CHF" });
            Assert.Equal("3.0/currencies/7", this._api.Resource);
            Assert.Equal(Method.PUT, this._api.LastMethod);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, "{\"success\":true}", true)]
        [InlineData(HttpStatusCode.OK, "{\"success\":false}", false)]
        [InlineData(HttpStatusCode.NoContent, "", true)]
        public void Delete_result(HttpStatusCode code, string content, bool expected)
        {
            this._api.StatusCode = code;
            this._api.Content = content;
            Assert.Equal(expected, new BexioApiContactEndpoint(this._api).Delete(3));
            Assert.Equal(Method.DELETE, this._api.LastMethod);
            Assert.Equal("2.0/contact/3", this._api.Resource);
        }

        [Fact]
        public void Invoice_actions()
        {
            var invoices = new BexioApiInvoiceEndpoint(this._api);
            invoices.Issue(1);
            Assert.Equal("2.0/kb_invoice/1/issue", this._api.Resource);
            invoices.MarkSent(1);
            Assert.Equal("2.0/kb_invoice/1/mark_as_sent", this._api.Resource);
            invoices.Send(1, new BexioInvoiceSend { recipient_email = "a@b.ch" });
            Assert.Equal("2.0/kb_invoice/1/send", this._api.Resource);
            Assert.Contains("a@b.ch", this._api.Body);
        }

        [Fact]
        public void Order_creates_invoice_on_correct_url()
        {
            new BexioApiOrderEndpoint(this._api).CreateInvoice(4, new BexioOrderInvoiceUpdate());
            Assert.Equal("2.0/kb_order/4/invoice", this._api.Resource);
        }

        [Fact]
        public void Positions_use_snake_case_segment()
        {
            this._api.Content = "[]";
            new BexioApiInvoicePositionEndpoint(this._api).GetAll(9, BexioPositionType.KbPositionCustom);
            Assert.Equal("2.0/kb_invoice/9/kb_position_custom", this._api.Resource);
        }

        [Fact]
        public void Error_response_throws_with_message_and_status()
        {
            this._api.StatusCode = HttpStatusCode.NotFound;
            this._api.Content = "{\"error_code\":404,\"message\":\"Not found\"}";
            var ex = Assert.Throws<BexioApiException>(() => new BexioApiContactEndpoint(this._api).GetById(1));
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.Contains("Not found", ex.Message);
        }

        [Fact]
        public void Non_json_error_does_not_break_exception()
        {
            this._api.StatusCode = HttpStatusCode.BadGateway;
            this._api.Content = "<html>bad gateway</html>";
            var ex = Assert.Throws<BexioApiException>(() => new BexioApiContactEndpoint(this._api).GetById(1));
            Assert.Contains("bad gateway", ex.Message);
        }

        [Fact]
        public async Task Async_variants()
        {
            this._api.Content = "{\"id\":2}";
            var contact = await new BexioApiContactEndpoint(this._api).GetByIdAsync(2);
            Assert.Equal(2, contact.id);
            Assert.Equal("2.0/contact/2", this._api.Resource);
        }

        [Theory]
        [InlineData("https://api.bexio.com/2.0", "https://api.bexio.com")]
        [InlineData("https://api.bexio.com/3.0/", "https://api.bexio.com")]
        [InlineData("https://api.bexio.com", "https://api.bexio.com")]
        [InlineData(null, "https://api.bexio.com")]
        public void Base_url_is_normalized(string input, string expected)
        {
            Assert.Equal(expected, BexioApi.NormalizeBaseUrl(input));
        }

        [Fact]
        public void Request_builds_to_correct_absolute_uri()
        {
            var api = BexioApi.UseJwt("https://api.bexio.com/2.0", "token");
            var uri = api.CLIENT.BuildUri(new RestRequest("3.0/accounting/calendar_years", DataFormat.Json));
            Assert.Equal("https://api.bexio.com/3.0/accounting/calendar_years", uri.ToString());
        }

        [Fact]
        public void All_endpoints_are_registered_in_di()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IBexioApi>(this._api);
            services.AddBexioEndpoints();
            var provider = services.BuildServiceProvider();

            var endpointInterfaces = typeof(IBexioApi).Assembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("IBexioApi") && t.Name.EndsWith("Endpoint") && !t.IsGenericType && t.Name != "IBexioApiEndpoint" && t.Name != "IBexioApiPositionEndpoint");
            foreach (var i in endpointInterfaces)
            {
                Assert.NotNull(provider.GetService(i));
            }
        }
    }
}
