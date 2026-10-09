using bexio_lib.Data;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiInvoicePaymentEndpoint : BexioApiEndpoint, IBexioApiInvoicePaymentEndpoint
    {
        public BexioApiInvoicePaymentEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "kb_invoice") { }

        private RestRequest Req(int invoiceId, string suffix = null)
            => this.NewRequest(suffix == null ? $"{invoiceId}/payment" : $"{invoiceId}/payment/{suffix}");

        public ICollection<BexioInvoicePayment> GetAll(int invoiceId)
            => this.Send<ICollection<BexioInvoicePayment>>(this.Req(invoiceId), Method.GET);

        public BexioInvoicePayment GetById(int invoiceId, int paymentId)
            => this.Send<BexioInvoicePayment>(this.Req(invoiceId, paymentId.ToString()), Method.GET);

        public BexioInvoicePayment Create(int invoiceId, BexioInvoicePayment payment)
            => this.Send<BexioInvoicePayment>(this.Req(invoiceId).AddRequestBodyData(payment), Method.POST);

        public bool Delete(int invoiceId, int paymentId)
            => this.API.Execute(this.Req(invoiceId, paymentId.ToString()), Method.DELETE).ToDeleteResult();

        public Task<ICollection<BexioInvoicePayment>> GetAllAsync(int invoiceId, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<BexioInvoicePayment>>(this.Req(invoiceId), Method.GET, cancellationToken);

        public Task<BexioInvoicePayment> CreateAsync(int invoiceId, BexioInvoicePayment payment, CancellationToken cancellationToken = default)
            => this.SendAsync<BexioInvoicePayment>(this.Req(invoiceId).AddRequestBodyData(payment), Method.POST, cancellationToken);
    }

    public abstract class BexioApiPositionEndpoint : BexioApiEndpoint, IBexioApiPositionEndpoint
    {
        protected BexioApiPositionEndpoint(IBexioApi api, string documentEndpoint) : base(api, BexioApiVersion.V2, documentEndpoint) { }

        private RestRequest Req(int documentId, string positionType, string suffix = null)
            => this.NewRequest(suffix == null ? $"{documentId}/{ToSegment(positionType)}" : $"{documentId}/{ToSegment(positionType)}/{suffix}");

        /// <summary>
        /// Position types may be passed as the resource segment ("kb_position_custom") directly; the segment is the type name in snake case.
        /// </summary>
        private static string ToSegment(string positionType)
        {
            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < positionType.Length; i++)
            {
                var c = positionType[i];
                if (char.IsUpper(c) && i > 0)
                {
                    sb.Append('_');
                }
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        public ICollection<BexioPosition> GetAll(int documentId, string positionType, BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<BexioPosition>>(this.Req(documentId, positionType).AddRequestData(requestParameter), Method.GET);

        public BexioPosition GetById(int documentId, string positionType, int positionId)
            => this.Send<BexioPosition>(this.Req(documentId, positionType, positionId.ToString()), Method.GET);

        public BexioPosition Create(int documentId, string positionType, BexioPosition position)
            => this.Send<BexioPosition>(this.Req(documentId, positionType).AddRequestBodyData(position), Method.POST);

        public BexioPosition Update(int documentId, string positionType, int positionId, BexioPosition position)
            => this.Send<BexioPosition>(this.Req(documentId, positionType, positionId.ToString()).AddRequestBodyData(position), Method.POST);

        public bool Delete(int documentId, string positionType, int positionId)
            => this.API.Execute(this.Req(documentId, positionType, positionId.ToString()), Method.DELETE).ToDeleteResult();

        public Task<ICollection<BexioPosition>> GetAllAsync(int documentId, string positionType, BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<BexioPosition>>(this.Req(documentId, positionType).AddRequestData(requestParameter), Method.GET, cancellationToken);

        public Task<BexioPosition> CreateAsync(int documentId, string positionType, BexioPosition position, CancellationToken cancellationToken = default)
            => this.SendAsync<BexioPosition>(this.Req(documentId, positionType).AddRequestBodyData(position), Method.POST, cancellationToken);
    }

    public class BexioApiInvoicePositionEndpoint : BexioApiPositionEndpoint, IBexioApiInvoicePositionEndpoint
    {
        public BexioApiInvoicePositionEndpoint(IBexioApi api) : base(api, "kb_invoice") { }
    }

    public class BexioApiOrderPositionEndpoint : BexioApiPositionEndpoint, IBexioApiOrderPositionEndpoint
    {
        public BexioApiOrderPositionEndpoint(IBexioApi api) : base(api, "kb_order") { }
    }

    public class BexioApiOfferPositionEndpoint : BexioApiPositionEndpoint, IBexioApiOfferPositionEndpoint
    {
        public BexioApiOfferPositionEndpoint(IBexioApi api) : base(api, "kb_offer") { }
    }

    public class BexioApiAdditionalAddressEndpoint : BexioApiEndpoint, IBexioApiAdditionalAddressEndpoint
    {
        public BexioApiAdditionalAddressEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "contact") { }

        private RestRequest Req(int contactId, string suffix = null)
            => this.NewRequest(suffix == null ? $"{contactId}/additional_address" : $"{contactId}/additional_address/{suffix}");

        public ICollection<BexioAdditionalAddress> GetAll(int contactId, BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<BexioAdditionalAddress>>(this.Req(contactId).AddRequestData(requestParameter), Method.GET);

        public BexioAdditionalAddress GetById(int contactId, int addressId)
            => this.Send<BexioAdditionalAddress>(this.Req(contactId, addressId.ToString()), Method.GET);

        public BexioAdditionalAddress Create(int contactId, BexioAdditionalAddress address)
            => this.Send<BexioAdditionalAddress>(this.Req(contactId).AddRequestBodyData(address), Method.POST);

        public BexioAdditionalAddress Update(int contactId, int addressId, BexioAdditionalAddress address)
            => this.Send<BexioAdditionalAddress>(this.Req(contactId, addressId.ToString()).AddRequestBodyData(address), Method.POST);

        public bool Delete(int contactId, int addressId)
            => this.API.Execute(this.Req(contactId, addressId.ToString()), Method.DELETE).ToDeleteResult();
    }
}
