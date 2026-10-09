using bexio_lib.Implementation.Endpoints;
using bexio_lib.Implementation.Endpoints.V3;
using bexio_lib.Interfaces;
using bexio_lib.Interfaces.V3;

namespace bexio_lib.Implementation
{
    // Generated-style facade: one property per endpoint, grouped by api version.
    // Endpoints are created lazily and cached per client.

    public interface IBexioV2
    {
        IBexioApiAccountGroupEndpoint AccountGroups { get; }
        IBexioApiAccountEndpoint Accounts { get; }
        IBexioApiAdditionalAddressEndpoint AdditionalAddresses { get; }
        IBexioApiArticleTypeEndpoint ArticleTypes { get; }
        IBexioApiArticleEndpoint Articles { get; }
        IBexioApiClientServiceEndpoint ClientServices { get; }
        IBexioApiCommunicationKindEndpoint CommunicationKinds { get; }
        IBexioApiCompanyProfileEndpoint CompanyProfile { get; }
        IBexioApiContactGroupEndpoint ContactGroups { get; }
        IBexioApiContactRelationEndpoint ContactRelations { get; }
        IBexioApiContactSectorEndpoint ContactSectors { get; }
        IBexioApiContactEndpoint Contacts { get; }
        IBexioApiCountryEndpoint Countries { get; }
        IBexioApiCurrencyEndpoint Currencies { get; }
        IBexioApiDeliveryEndpoint Deliveries { get; }
        IBexioApiFictionalUserEndpoint FictionalUsers { get; }
        IBexioApiInvoicePaymentEndpoint InvoicePayments { get; }
        IBexioApiInvoicePositionEndpoint InvoicePositions { get; }
        IBexioApiInvoiceEndpoint Invoices { get; }
        IBexioApiLanguageEndpoint Languages { get; }
        IBexioApiNoteEndpoint Notes { get; }
        IBexioApiOfferPositionEndpoint OfferPositions { get; }
        IBexioApiOfferEndpoint Offers { get; }
        IBexioApiOrderPositionEndpoint OrderPositions { get; }
        IBexioApiOrderEndpoint Orders { get; }
        IBexioApiPaymentTypeEndpoint PaymentTypes { get; }
        IBexioApiProjectStatusEndpoint ProjectStatuses { get; }
        IBexioApiProjectTypeEndpoint ProjectTypes { get; }
        IBexioApiProjectEndpoint Projects { get; }
        IBexioApiSalutationEndpoint Salutations { get; }
        IBexioApiStockPlaceEndpoint StockPlaces { get; }
        IBexioApiStockEndpoint Stocks { get; }
        IBexioApiTaskEndpoint Tasks { get; }
        IBexioApiTimesheetStatusEndpoint TimesheetStatuses { get; }
        IBexioApiTimesheetEndpoint Timesheets { get; }
        IBexioApiTitleEndpoint Titles { get; }
        IBexioApiUnitEndpoint Units { get; }
        IBexioApiUserEndpoint Users { get; }
    }

    public class BexioV2 : IBexioV2
    {
        private readonly IBexioApi _api;
        private IBexioApiAccountGroupEndpoint _AccountGroups;
        private IBexioApiAccountEndpoint _Accounts;
        private IBexioApiAdditionalAddressEndpoint _AdditionalAddresses;
        private IBexioApiArticleTypeEndpoint _ArticleTypes;
        private IBexioApiArticleEndpoint _Articles;
        private IBexioApiClientServiceEndpoint _ClientServices;
        private IBexioApiCommunicationKindEndpoint _CommunicationKinds;
        private IBexioApiCompanyProfileEndpoint _CompanyProfile;
        private IBexioApiContactGroupEndpoint _ContactGroups;
        private IBexioApiContactRelationEndpoint _ContactRelations;
        private IBexioApiContactSectorEndpoint _ContactSectors;
        private IBexioApiContactEndpoint _Contacts;
        private IBexioApiCountryEndpoint _Countries;
        private IBexioApiCurrencyEndpoint _Currencies;
        private IBexioApiDeliveryEndpoint _Deliveries;
        private IBexioApiFictionalUserEndpoint _FictionalUsers;
        private IBexioApiInvoicePaymentEndpoint _InvoicePayments;
        private IBexioApiInvoicePositionEndpoint _InvoicePositions;
        private IBexioApiInvoiceEndpoint _Invoices;
        private IBexioApiLanguageEndpoint _Languages;
        private IBexioApiNoteEndpoint _Notes;
        private IBexioApiOfferPositionEndpoint _OfferPositions;
        private IBexioApiOfferEndpoint _Offers;
        private IBexioApiOrderPositionEndpoint _OrderPositions;
        private IBexioApiOrderEndpoint _Orders;
        private IBexioApiPaymentTypeEndpoint _PaymentTypes;
        private IBexioApiProjectStatusEndpoint _ProjectStatuses;
        private IBexioApiProjectTypeEndpoint _ProjectTypes;
        private IBexioApiProjectEndpoint _Projects;
        private IBexioApiSalutationEndpoint _Salutations;
        private IBexioApiStockPlaceEndpoint _StockPlaces;
        private IBexioApiStockEndpoint _Stocks;
        private IBexioApiTaskEndpoint _Tasks;
        private IBexioApiTimesheetStatusEndpoint _TimesheetStatuses;
        private IBexioApiTimesheetEndpoint _Timesheets;
        private IBexioApiTitleEndpoint _Titles;
        private IBexioApiUnitEndpoint _Units;
        private IBexioApiUserEndpoint _Users;

        public BexioV2(IBexioApi api)
        {
            this._api = api;
        }

        public IBexioApiAccountGroupEndpoint AccountGroups => this._AccountGroups ??= new BexioApiAccountGroupEndpoint(this._api);
        public IBexioApiAccountEndpoint Accounts => this._Accounts ??= new BexioApiAccountEndpoint(this._api);
        public IBexioApiAdditionalAddressEndpoint AdditionalAddresses => this._AdditionalAddresses ??= new BexioApiAdditionalAddressEndpoint(this._api);
        public IBexioApiArticleTypeEndpoint ArticleTypes => this._ArticleTypes ??= new BexioApiArticleTypeEndpoint(this._api);
        public IBexioApiArticleEndpoint Articles => this._Articles ??= new BexioApiArticleEndpoint(this._api);
        public IBexioApiClientServiceEndpoint ClientServices => this._ClientServices ??= new BexioApiClientServiceEndpoint(this._api);
        public IBexioApiCommunicationKindEndpoint CommunicationKinds => this._CommunicationKinds ??= new BexioApiCommunicationKindEndpoint(this._api);
        public IBexioApiCompanyProfileEndpoint CompanyProfile => this._CompanyProfile ??= new BexioApiCompanyProfileEndpoint(this._api);
        public IBexioApiContactGroupEndpoint ContactGroups => this._ContactGroups ??= new BexioApiContactGroupEndpoint(this._api);
        public IBexioApiContactRelationEndpoint ContactRelations => this._ContactRelations ??= new BexioApiContactRelationEndpoint(this._api);
        public IBexioApiContactSectorEndpoint ContactSectors => this._ContactSectors ??= new BexioApiContactSectorEndpoint(this._api);
        public IBexioApiContactEndpoint Contacts => this._Contacts ??= new BexioApiContactEndpoint(this._api);
        public IBexioApiCountryEndpoint Countries => this._Countries ??= new BexioApiCountryEndpoint(this._api);
        public IBexioApiCurrencyEndpoint Currencies => this._Currencies ??= new BexioApiCurrencyEndpoint(this._api);
        public IBexioApiDeliveryEndpoint Deliveries => this._Deliveries ??= new BexioApiDeliveryEndpoint(this._api);
        public IBexioApiFictionalUserEndpoint FictionalUsers => this._FictionalUsers ??= new BexioApiFictionalUserEndpoint(this._api);
        public IBexioApiInvoicePaymentEndpoint InvoicePayments => this._InvoicePayments ??= new BexioApiInvoicePaymentEndpoint(this._api);
        public IBexioApiInvoicePositionEndpoint InvoicePositions => this._InvoicePositions ??= new BexioApiInvoicePositionEndpoint(this._api);
        public IBexioApiInvoiceEndpoint Invoices => this._Invoices ??= new BexioApiInvoiceEndpoint(this._api);
        public IBexioApiLanguageEndpoint Languages => this._Languages ??= new BexioApiLanguageEndpoint(this._api);
        public IBexioApiNoteEndpoint Notes => this._Notes ??= new BexioApiNoteEndpoint(this._api);
        public IBexioApiOfferPositionEndpoint OfferPositions => this._OfferPositions ??= new BexioApiOfferPositionEndpoint(this._api);
        public IBexioApiOfferEndpoint Offers => this._Offers ??= new BexioApiOfferEndpoint(this._api);
        public IBexioApiOrderPositionEndpoint OrderPositions => this._OrderPositions ??= new BexioApiOrderPositionEndpoint(this._api);
        public IBexioApiOrderEndpoint Orders => this._Orders ??= new BexioApiOrderEndpoint(this._api);
        public IBexioApiPaymentTypeEndpoint PaymentTypes => this._PaymentTypes ??= new BexioApiPaymentTypeEndpoint(this._api);
        public IBexioApiProjectStatusEndpoint ProjectStatuses => this._ProjectStatuses ??= new BexioApiProjectStatusEndpoint(this._api);
        public IBexioApiProjectTypeEndpoint ProjectTypes => this._ProjectTypes ??= new BexioApiProjectTypeEndpoint(this._api);
        public IBexioApiProjectEndpoint Projects => this._Projects ??= new BexioApiProjectEndpoint(this._api);
        public IBexioApiSalutationEndpoint Salutations => this._Salutations ??= new BexioApiSalutationEndpoint(this._api);
        public IBexioApiStockPlaceEndpoint StockPlaces => this._StockPlaces ??= new BexioApiStockPlaceEndpoint(this._api);
        public IBexioApiStockEndpoint Stocks => this._Stocks ??= new BexioApiStockEndpoint(this._api);
        public IBexioApiTaskEndpoint Tasks => this._Tasks ??= new BexioApiTaskEndpoint(this._api);
        public IBexioApiTimesheetStatusEndpoint TimesheetStatuses => this._TimesheetStatuses ??= new BexioApiTimesheetStatusEndpoint(this._api);
        public IBexioApiTimesheetEndpoint Timesheets => this._Timesheets ??= new BexioApiTimesheetEndpoint(this._api);
        public IBexioApiTitleEndpoint Titles => this._Titles ??= new BexioApiTitleEndpoint(this._api);
        public IBexioApiUnitEndpoint Units => this._Units ??= new BexioApiUnitEndpoint(this._api);
        public IBexioApiUserEndpoint Users => this._Users ??= new BexioApiUserEndpoint(this._api);
    }

    public interface IBexioV3
    {
        IBexioApiBankAccountEndpoint BankAccounts { get; }
        IBexioApiBusinessYearEndpoint BusinessYears { get; }
        IBexioApiCalendarYearEndpoint CalendarYears { get; }
        IBexioApiCurrencyV3Endpoint Currencies { get; }
        IBexioApiFileEndpoint Files { get; }
        IBexioApiJournalEndpoint Journal { get; }
        IBexioApiManualEntryEndpoint ManualEntries { get; }
        IBexioApiTaxEndpoint Taxes { get; }
        IBexioApiUserV3Endpoint Users { get; }
        IBexioApiVatPeriodEndpoint VatPeriods { get; }
    }

    public class BexioV3 : IBexioV3
    {
        private readonly IBexioApi _api;
        private IBexioApiBankAccountEndpoint _BankAccounts;
        private IBexioApiBusinessYearEndpoint _BusinessYears;
        private IBexioApiCalendarYearEndpoint _CalendarYears;
        private IBexioApiCurrencyV3Endpoint _Currencies;
        private IBexioApiFileEndpoint _Files;
        private IBexioApiJournalEndpoint _Journal;
        private IBexioApiManualEntryEndpoint _ManualEntries;
        private IBexioApiTaxEndpoint _Taxes;
        private IBexioApiUserV3Endpoint _Users;
        private IBexioApiVatPeriodEndpoint _VatPeriods;

        public BexioV3(IBexioApi api)
        {
            this._api = api;
        }

        public IBexioApiBankAccountEndpoint BankAccounts => this._BankAccounts ??= new BexioApiBankAccountEndpoint(this._api);
        public IBexioApiBusinessYearEndpoint BusinessYears => this._BusinessYears ??= new BexioApiBusinessYearEndpoint(this._api);
        public IBexioApiCalendarYearEndpoint CalendarYears => this._CalendarYears ??= new BexioApiCalendarYearEndpoint(this._api);
        public IBexioApiCurrencyV3Endpoint Currencies => this._Currencies ??= new BexioApiCurrencyV3Endpoint(this._api);
        public IBexioApiFileEndpoint Files => this._Files ??= new BexioApiFileEndpoint(this._api);
        public IBexioApiJournalEndpoint Journal => this._Journal ??= new BexioApiJournalEndpoint(this._api);
        public IBexioApiManualEntryEndpoint ManualEntries => this._ManualEntries ??= new BexioApiManualEntryEndpoint(this._api);
        public IBexioApiTaxEndpoint Taxes => this._Taxes ??= new BexioApiTaxEndpoint(this._api);
        public IBexioApiUserV3Endpoint Users => this._Users ??= new BexioApiUserV3Endpoint(this._api);
        public IBexioApiVatPeriodEndpoint VatPeriods => this._VatPeriods ??= new BexioApiVatPeriodEndpoint(this._api);
    }
}
