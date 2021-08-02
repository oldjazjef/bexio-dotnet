using bexio_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    public interface IBexioApiOrderEndpoint : IBexioApiFullEndpoint<BexioOrder>
    {
        public BexioInvoice CreateInvoice(int orderId, BexioOrderInvoiceUpdate invoice);
        public BexioDelivery CreateDelivery(int orderId, BexioOrderDeliveryUpdate delivery);
    }

    public interface IBexioApiContactEndpoint : IBexioApiFullEndpoint<BexioContact>
    {
    }

    public interface IBexioApiCountryEndpoint : IBexioApiFullEndpoint<BexioCountry>
    {
    }

    public interface IBexioApiCurrencyEndpoint : IBexioApiFullEndpoint<BexioCurrency>
    {
    }

    public interface IBexioApiArticleEndpoint : IBexioApiFullEndpoint<BexioItem>
    {
    }

    public interface IBexioApiDeliveryEndpoint : IBexioApiFullEndpoint<BexioDelivery>
    {
        public void Issue(int deliveryId);
    }

    public interface IBexioApiInvoiceEndpoint : IBexioApiFullEndpoint<BexioInvoice>
    {
        public void Send(int invoiceId, BexioInvoiceSend data);
        public void MarkSent(int id);
        public void Issue(int invoiceId);
        public BexioPdf GetPdf(int invoiceId);

    }
}
