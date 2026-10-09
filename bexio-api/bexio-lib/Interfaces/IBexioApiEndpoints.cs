using bexio_lib.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    // ---------- Sales (api 2.0) ----------

    public interface IBexioApiOrderEndpoint : IBexioApiCrudEndpoint<BexioOrder>
    {
        BexioInvoice CreateInvoice(int orderId, BexioOrderInvoiceUpdate invoice);
        BexioDelivery CreateDelivery(int orderId, BexioOrderDeliveryUpdate delivery);
        BexioPdf GetPdf(int orderId);
    }

    public interface IBexioApiOfferEndpoint : IBexioApiCrudEndpoint<BexioOffer>
    {
        void Issue(int offerId);
        void Revoke(int offerId);
        void Accept(int offerId);
        void Reject(int offerId);
        void Reissue(int offerId);
        void MarkSent(int offerId);
        void Send(int offerId, BexioInvoiceSend data);
        BexioPdf GetPdf(int offerId);
        BexioOrder CreateOrder(int offerId);
        BexioInvoice CreateInvoice(int offerId);
    }

    public interface IBexioApiInvoiceEndpoint : IBexioApiCrudEndpoint<BexioInvoice>
    {
        void Send(int invoiceId, BexioInvoiceSend data);
        void MarkSent(int id);
        void Issue(int invoiceId);
        void Revoke(int invoiceId);
        void Cancel(int invoiceId);
        BexioPdf GetPdf(int invoiceId);
    }

    public interface IBexioApiDeliveryEndpoint : IBexioApiFullEndpoint<BexioDelivery>
    {
        void Issue(int deliveryId);
    }

    /// <summary>
    /// Payments of an invoice: kb_invoice/{invoiceId}/payment
    /// </summary>
    public interface IBexioApiInvoicePaymentEndpoint : IBexioApiEndpoint
    {
        ICollection<BexioInvoicePayment> GetAll(int invoiceId);
        BexioInvoicePayment GetById(int invoiceId, int paymentId);
        BexioInvoicePayment Create(int invoiceId, BexioInvoicePayment payment);
        bool Delete(int invoiceId, int paymentId);

        Task<ICollection<BexioInvoicePayment>> GetAllAsync(int invoiceId, CancellationToken cancellationToken = default);
        Task<BexioInvoicePayment> CreateAsync(int invoiceId, BexioInvoicePayment payment, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Positions of a sales document, e.g. kb_invoice/{documentId}/kb_position_custom.
    /// The position type is one of <see cref="bexio_lib.Data.BexioPositionType"/>.
    /// </summary>
    public interface IBexioApiPositionEndpoint : IBexioApiEndpoint
    {
        ICollection<BexioPosition> GetAll(int documentId, string positionType, BexioRequestFilter requestParameter = null);
        BexioPosition GetById(int documentId, string positionType, int positionId);
        BexioPosition Create(int documentId, string positionType, BexioPosition position);
        BexioPosition Update(int documentId, string positionType, int positionId, BexioPosition position);
        bool Delete(int documentId, string positionType, int positionId);

        Task<ICollection<BexioPosition>> GetAllAsync(int documentId, string positionType, BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default);
        Task<BexioPosition> CreateAsync(int documentId, string positionType, BexioPosition position, CancellationToken cancellationToken = default);
    }

    public interface IBexioApiInvoicePositionEndpoint : IBexioApiPositionEndpoint { }
    public interface IBexioApiOrderPositionEndpoint : IBexioApiPositionEndpoint { }
    public interface IBexioApiOfferPositionEndpoint : IBexioApiPositionEndpoint { }

    // ---------- Contacts (api 2.0) ----------

    public interface IBexioApiContactEndpoint : IBexioApiCrudEndpoint<BexioContact> { }
    public interface IBexioApiContactRelationEndpoint : IBexioApiCrudEndpoint<BexioContactRelation> { }
    public interface IBexioApiContactGroupEndpoint : IBexioApiCrudEndpoint<BexioContactGroup> { }
    public interface IBexioApiContactSectorEndpoint : IBexioApiFullEndpoint<BexioContactSector> { }
    public interface IBexioApiSalutationEndpoint : IBexioApiCrudEndpoint<BexioSalutation> { }
    public interface IBexioApiTitleEndpoint : IBexioApiCrudEndpoint<BexioTitle> { }

    /// <summary>
    /// Additional addresses of a contact: contact/{contactId}/additional_address
    /// </summary>
    public interface IBexioApiAdditionalAddressEndpoint : IBexioApiEndpoint
    {
        ICollection<BexioAdditionalAddress> GetAll(int contactId, BexioRequestFilter requestParameter = null);
        BexioAdditionalAddress GetById(int contactId, int addressId);
        BexioAdditionalAddress Create(int contactId, BexioAdditionalAddress address);
        BexioAdditionalAddress Update(int contactId, int addressId, BexioAdditionalAddress address);
        bool Delete(int contactId, int addressId);
    }

    // ---------- Items (api 2.0) ----------

    public interface IBexioApiArticleEndpoint : IBexioApiCrudEndpoint<BexioItem> { }
    public interface IBexioApiArticleTypeEndpoint : IBexioApiFullEndpoint<BexioArticleType> { }
    public interface IBexioApiUnitEndpoint : IBexioApiCrudEndpoint<BexioUnit> { }
    public interface IBexioApiStockEndpoint : IBexioApiFullEndpoint<BexioStock> { }
    public interface IBexioApiStockPlaceEndpoint : IBexioApiFullEndpoint<BexioStockPlace> { }

    // ---------- Projects / time tracking / notes (api 2.0) ----------

    public interface IBexioApiProjectEndpoint : IBexioApiCrudEndpoint<BexioProject>
    {
        void Archive(int projectId);
        void Unarchive(int projectId);
    }
    public interface IBexioApiProjectTypeEndpoint : IBexioApiFullEndpoint<BexioProjectType> { }
    public interface IBexioApiProjectStatusEndpoint : IBexioApiFullEndpoint<BexioProjectStatus> { }
    public interface IBexioApiTimesheetEndpoint : IBexioApiCrudEndpoint<BexioTimesheet> { }
    public interface IBexioApiTimesheetStatusEndpoint : IBexioApiFullEndpoint<BexioTimesheetStatus> { }
    public interface IBexioApiClientServiceEndpoint : IBexioApiCrudEndpoint<BexioClientService> { }
    public interface IBexioApiCommunicationKindEndpoint : IBexioApiFullEndpoint<BexioCommunicationKind> { }
    public interface IBexioApiNoteEndpoint : IBexioApiCrudEndpoint<BexioNote> { }
    public interface IBexioApiTaskEndpoint : IBexioApiCrudEndpoint<BexioTask> { }

    // ---------- Accounting (api 2.0) ----------

    public interface IBexioApiAccountEndpoint : IBexioApiFullEndpoint<BexioAccount> { }
    public interface IBexioApiAccountGroupEndpoint : IBexioApiFullEndpoint<BexioAccountGroup> { }

    // ---------- Other (api 2.0) ----------

    public interface IBexioApiCountryEndpoint : IBexioApiCrudEndpoint<BexioCountry> { }
    public interface IBexioApiCurrencyEndpoint : IBexioApiFullEndpoint<BexioCurrency> { }
    public interface IBexioApiLanguageEndpoint : IBexioApiCrudEndpoint<BexioLanguage> { }
    public interface IBexioApiPaymentTypeEndpoint : IBexioApiFullEndpoint<BexioPaymentType> { }
    public interface IBexioApiUserEndpoint : IBexioApiFullEndpoint<BexioUser> { }
    public interface IBexioApiFictionalUserEndpoint : IBexioApiCrudEndpoint<BexioFictionalUser> { }
    public interface IBexioApiCompanyProfileEndpoint : IBexioApiFullEndpoint<BexioCompanyProfile> { }
}
