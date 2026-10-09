using bexio_lib.Data;
using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BexioLibTest.Unit
{
    public class EndpointTests
    {
        private readonly FakeBexioApi _api = new FakeBexioApi();
        private IBexioClient Client => new BexioClient(this._api);

        [Fact]
        public void Versions_are_part_of_the_resource()
        {
            this.Client.V2.Contacts.GetAll();
            Assert.Equal("2.0/contact", this._api.Resource);
            Assert.Equal(HttpMethod.Get, this._api.LastMethod);

            this.Client.V3.Currencies.GetAll();
            Assert.Equal("3.0/currencies", this._api.Resource);
        }

        [Fact]
        public void GetAll_sets_query_parameters()
        {
            this.Client.V2.Contacts.GetAll(new BexioRequestFilter { limit = 10, offset = 20, order_by = new[] { "id_desc", "name" } });
            Assert.Equal("10", this._api.Query("limit"));
            Assert.Equal("20", this._api.Query("offset"));
            Assert.Equal("id_desc,name", this._api.Query("order_by"));
        }

        [Fact]
        public void Typed_query_parameters_are_sent_and_null_is_skipped()
        {
            this.Client.V2.Contacts.ListContacts(showArchived: true);
            Assert.Equal("true", this._api.Query("show_archived"));

            this.Client.V2.Contacts.ListContacts();
            Assert.Null(this._api.Query("show_archived"));
        }

        [Fact]
        public void Search_posts_filters_as_array()
        {
            var filter = new BexioRequestFilter().Add(new BexioRequestFilterInstruction(BexioSearchFields.KbInvoice.kb_item_status_id, "9,16", BexioFilterCriteria.IN));
            this.Client.V2.Invoices.Search(filter);
            Assert.Equal("2.0/kb_invoice/search", this._api.Resource);
            Assert.Equal(HttpMethod.Post, this._api.LastMethod);
            Assert.Equal("[{\"field\":\"kb_item_status_id\",\"value\":\"9,16\",\"criteria\":\"in\"}]", this._api.Body);
        }

        [Fact]
        public void Search_without_filter_sends_empty_array()
        {
            this.Client.V2.Invoices.Search();
            Assert.Equal("[]", this._api.Body);
        }

        [Fact]
        public void Create_omits_null_properties()
        {
            this._api.Content = "{\"id\":5}";
            var result = this.Client.V2.Contacts.Create(new BexioContactRequest { name_1 = "Muster", contact_type_id = 1 });
            Assert.Equal(5, result.id);
            Assert.Equal(HttpMethod.Post, this._api.LastMethod);
            Assert.DoesNotContain("null", this._api.Body);
            Assert.Contains("\"name_1\":\"Muster\"", this._api.Body);
        }

        [Fact]
        public void Update_uses_the_http_method_of_the_api_version()
        {
            this.Client.V2.Contacts.Update(7, new BexioContactRequest { name_1 = "x" });
            Assert.Equal("2.0/contact/7", this._api.Resource);
            Assert.Equal(HttpMethod.Post, this._api.LastMethod);

            this.Client.V3.Currencies.UpdateCurrency(7, new Newtonsoft.Json.Linq.JObject { ["name"] = "CHF" });
            Assert.Equal("3.0/currencies/7", this._api.Resource);
            Assert.Equal(HttpMethod.Patch, this._api.LastMethod);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, "{\"success\":true}", true)]
        [InlineData(HttpStatusCode.OK, "{\"success\":false}", false)]
        [InlineData(HttpStatusCode.NoContent, "", true)]
        public void Delete_result(HttpStatusCode code, string content, bool expected)
        {
            this._api.StatusCode = code;
            this._api.Content = content;
            Assert.Equal(expected, this.Client.V2.Contacts.Delete(3));
            Assert.Equal(HttpMethod.Delete, this._api.LastMethod);
            Assert.Equal("2.0/contact/3", this._api.Resource);
        }

        [Fact]
        public void Invoice_actions_use_the_documented_paths()
        {
            var invoices = this.Client.V2.Invoices;
            invoices.IssueInvoice(1);
            Assert.Equal("2.0/kb_invoice/1/issue", this._api.Resource);
            invoices.MarkAsSentInvoice(1);
            Assert.Equal("2.0/kb_invoice/1/mark_as_sent", this._api.Resource);
            invoices.RevertIssueInvoice(1);
            Assert.Equal("2.0/kb_invoice/1/revert_issue", this._api.Resource);
            invoices.ShowInvoicePDF(1, 5);
            Assert.Equal("2.0/kb_invoice/1/pdf", this._api.Resource);
        }

        [Fact]
        public void Document_type_is_a_path_parameter_of_positions()
        {
            this.Client.V2.DefaultPositions.ListDefaultPositions("kb_invoice", 9);
            Assert.Equal("2.0/kb_invoice/9/kb_position_custom", this._api.Resource);
        }

        [Fact]
        public void Error_response_throws_with_message_and_status()
        {
            this._api.StatusCode = HttpStatusCode.NotFound;
            this._api.Content = "{\"error_code\":404,\"message\":\"Not found\"}";
            var ex = Assert.Throws<BexioApiException>(() => this.Client.V2.Contacts.GetById(1));
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.Contains("Not found", ex.Message);
        }

        [Fact]
        public void Non_json_error_does_not_break_exception()
        {
            this._api.StatusCode = HttpStatusCode.BadGateway;
            this._api.Content = "<html>bad gateway</html>";
            var ex = Assert.Throws<BexioApiException>(() => this.Client.V2.Contacts.GetById(1));
            Assert.Contains("bad gateway", ex.Message);
        }

        [Fact]
        public async Task Async_variants()
        {
            this._api.Content = "{\"id\":2}";
            var contact = await this.Client.V2.Contacts.GetByIdAsync(2);
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
        public void Client_groups_endpoints_by_version_and_caches_them()
        {
            var client = new BexioClient(this._api);
            client.V2.Contacts.GetAll();
            Assert.Equal("2.0/contact", this._api.Resource);
            client.V3.Taxes.ListTaxes();
            Assert.Equal("3.0/taxes", this._api.Resource);
            Assert.Same(client.V2.Contacts, client.V2.Contacts);
        }

        [Fact]
        public void All_endpoints_are_registered_in_di()
        {
            var services = new ServiceCollection();
            services.AddBexio(o => o.AccessToken = "t");
            var provider = services.BuildServiceProvider();

            var endpointInterfaces = typeof(IBexioApi).Assembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("IBexioApi") && t.Name.EndsWith("Endpoint") && !t.IsGenericType
                            && t.Name != "IBexioApiEndpoint" && t.Name != "IBexioApiPositionEndpoint").ToList();
            Assert.True(endpointInterfaces.Count >= 50);
            foreach (var i in endpointInterfaces)
            {
                Assert.NotNull(provider.GetService(i));
            }
            Assert.NotNull(provider.GetService<IBexioClient>());
            Assert.NotNull(provider.GetService<IBexioClientFactory>());
        }
    }
}
