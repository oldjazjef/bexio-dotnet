using bexio_lib.Data;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;
using System.Linq;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiOrderEndpoint : BexioApiCrudEndpoint<BexioOrder>, IBexioApiOrderEndpoint
    {
        public BexioApiOrderEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "kb_order") { }

        public BexioDelivery CreateDelivery(int orderId, BexioOrderDeliveryUpdate delivery)
        {
            var request = this.NewRequest($"{orderId}/delivery");
            if (delivery?.positions?.Any() == true)
            {
                request.AddRequestBodyData(delivery);
            }
            return this.Send<BexioDelivery>(request, Method.POST);
        }

        public BexioInvoice CreateInvoice(int orderId, BexioOrderInvoiceUpdate invoice)
        {
            var request = this.NewRequest($"{orderId}/invoice");
            if (invoice?.positions?.Any() == true)
            {
                request.AddRequestBodyData(invoice);
            }
            return this.Send<BexioInvoice>(request, Method.POST);
        }

        public BexioPdf GetPdf(int orderId)
            => this.Send<BexioPdf>(this.NewRequest($"{orderId}/pdf"), Method.GET);
    }

    public class BexioApiOfferEndpoint : BexioApiCrudEndpoint<BexioOffer>, IBexioApiOfferEndpoint
    {
        public BexioApiOfferEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "kb_offer") { }

        public void Issue(int offerId) => this.Send(this.NewRequest($"{offerId}/issue"), Method.POST);
        public void Revoke(int offerId) => this.Send(this.NewRequest($"{offerId}/revoke"), Method.POST);
        public void Accept(int offerId) => this.Send(this.NewRequest($"{offerId}/accept"), Method.POST);
        public void Reject(int offerId) => this.Send(this.NewRequest($"{offerId}/reject"), Method.POST);
        public void Reissue(int offerId) => this.Send(this.NewRequest($"{offerId}/reissue"), Method.POST);
        public void MarkSent(int offerId) => this.Send(this.NewRequest($"{offerId}/mark_as_sent"), Method.POST);

        public void Send(int offerId, BexioInvoiceSend data)
            => this.Send(this.NewRequest($"{offerId}/send").AddRequestBodyData(data), Method.POST);

        public BexioPdf GetPdf(int offerId)
            => this.Send<BexioPdf>(this.NewRequest($"{offerId}/pdf"), Method.GET);

        public BexioOrder CreateOrder(int offerId)
            => this.Send<BexioOrder>(this.NewRequest($"{offerId}/order"), Method.POST);

        public BexioInvoice CreateInvoice(int offerId)
            => this.Send<BexioInvoice>(this.NewRequest($"{offerId}/invoice"), Method.POST);
    }

    public class BexioApiInvoiceEndpoint : BexioApiCrudEndpoint<BexioInvoice>, IBexioApiInvoiceEndpoint
    {
        public BexioApiInvoiceEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "kb_invoice") { }

        public BexioPdf GetPdf(int invoiceId)
            => this.Send<BexioPdf>(this.NewRequest($"{invoiceId}/pdf"), Method.GET);

        public void Issue(int invoiceId) => this.Send(this.NewRequest($"{invoiceId}/issue"), Method.POST);
        public void Revoke(int invoiceId) => this.Send(this.NewRequest($"{invoiceId}/revoke"), Method.POST);
        public void Cancel(int invoiceId) => this.Send(this.NewRequest($"{invoiceId}/cancel"), Method.POST);
        public void MarkSent(int id) => this.Send(this.NewRequest($"{id}/mark_as_sent"), Method.POST);

        public void Send(int invoiceId, BexioInvoiceSend data)
            => this.Send(this.NewRequest($"{invoiceId}/send").AddRequestBodyData(data), Method.POST);
    }

    public class BexioApiDeliveryEndpoint : BexioApiFullEndpoint<BexioDelivery>, IBexioApiDeliveryEndpoint
    {
        public BexioApiDeliveryEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "kb_delivery") { }

        public void Issue(int deliveryId) => this.Send(this.NewRequest($"{deliveryId}/issue"), Method.POST);
    }
}
