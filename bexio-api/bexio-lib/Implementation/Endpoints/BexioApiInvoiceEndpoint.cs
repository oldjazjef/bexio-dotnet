using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiInvoiceEndpoint : BexioApiFullEndpoint<BexioInvoice>, IBexioApiInvoiceEndpoint
    {
        public BexioApiInvoiceEndpoint(IBexioApi api): base(api, "kb_invoice")
        {

        }

        public BexioPdf GetPdf(int invoiceId)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{invoiceId}/pdf ", DataFormat.Json);
            var result = this.API.Get(request)
                .DeserializeRequestResult<BexioPdf>();
            return result;
        }

        public void Issue(int invoiceId)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{invoiceId}/issue ", DataFormat.Json);
            this.API.Post(request);
        }

        public void MarkSent(int id)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{id}/mark_as_sent ", DataFormat.Json);
            this.API.Post(request);
        }

        public void Send(int invoiceId, BexioInvoiceSend data)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{invoiceId}/send ", DataFormat.Json)
                .AddRequestBodyData(data);
            this.API.Post(request);
        }
    }
}
