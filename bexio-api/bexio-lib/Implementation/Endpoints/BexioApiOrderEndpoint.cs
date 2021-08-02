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
    public class BexioApiOrderEndpoint : BexioApiFullEndpoint<BexioOrder>, IBexioApiOrderEndpoint
    {
        public BexioApiOrderEndpoint(IBexioApi api): base(api, "kb_order")
        {

        }

        public BexioDelivery CreateDelivery(int orderId, BexioOrderDeliveryUpdate delivery)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{orderId}/delivery", DataFormat.Json);

            if (delivery.positions?.Any() == true)
            {
                request = request.AddRequestBodyData(delivery);
            }

            var result = this.API.Post(request)
                .DeserializeRequestResult<BexioDelivery>();

            return result;
        }

        public BexioInvoice CreateInvoice(int orderId, BexioOrderInvoiceUpdate invoice)
        {
            var request = new RestRequest($"/2.0/kb_order/{orderId}/invoid", DataFormat.Json);

            if (invoice.positions?.Any() == true)
            {
                request = request.AddRequestBodyData(invoice);
            }

            var result = this.API.Post(request)
                .DeserializeRequestResult<BexioInvoice>();

            return result;
        }
    }
}
