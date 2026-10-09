using bexio_lib.Data;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;

namespace bexio_lib.Implementation.Endpoints
{
    // Contacts
    public class BexioApiContactEndpoint : BexioApiCrudEndpoint<BexioContact>, IBexioApiContactEndpoint
    {
        public BexioApiContactEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "contact") { }
    }
    public class BexioApiContactRelationEndpoint : BexioApiCrudEndpoint<BexioContactRelation>, IBexioApiContactRelationEndpoint
    {
        public BexioApiContactRelationEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "contact_relation") { }
    }
    public class BexioApiContactGroupEndpoint : BexioApiCrudEndpoint<BexioContactGroup>, IBexioApiContactGroupEndpoint
    {
        public BexioApiContactGroupEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "contact_group") { }
    }
    public class BexioApiContactSectorEndpoint : BexioApiFullEndpoint<BexioContactSector>, IBexioApiContactSectorEndpoint
    {
        public BexioApiContactSectorEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "contact_branch") { }
    }
    public class BexioApiSalutationEndpoint : BexioApiCrudEndpoint<BexioSalutation>, IBexioApiSalutationEndpoint
    {
        public BexioApiSalutationEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "salutation") { }
    }
    public class BexioApiTitleEndpoint : BexioApiCrudEndpoint<BexioTitle>, IBexioApiTitleEndpoint
    {
        public BexioApiTitleEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "title") { }
    }

    // Items
    public class BexioApiArticleEndpoint : BexioApiCrudEndpoint<BexioItem>, IBexioApiArticleEndpoint
    {
        public BexioApiArticleEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "article") { }
    }
    public class BexioApiArticleTypeEndpoint : BexioApiFullEndpoint<BexioArticleType>, IBexioApiArticleTypeEndpoint
    {
        public BexioApiArticleTypeEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "article_type") { }
    }
    public class BexioApiUnitEndpoint : BexioApiCrudEndpoint<BexioUnit>, IBexioApiUnitEndpoint
    {
        public BexioApiUnitEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "unit") { }
    }
    public class BexioApiStockEndpoint : BexioApiFullEndpoint<BexioStock>, IBexioApiStockEndpoint
    {
        public BexioApiStockEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "stock") { }
    }
    public class BexioApiStockPlaceEndpoint : BexioApiFullEndpoint<BexioStockPlace>, IBexioApiStockPlaceEndpoint
    {
        public BexioApiStockPlaceEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "stock_place") { }
    }

    // Projects, time tracking, notes
    public class BexioApiProjectEndpoint : BexioApiCrudEndpoint<BexioProject>, IBexioApiProjectEndpoint
    {
        public BexioApiProjectEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "pr_project") { }

        public void Archive(int projectId) => this.Send(this.NewRequest($"{projectId}/archive"), Method.POST);
        public void Unarchive(int projectId) => this.Send(this.NewRequest($"{projectId}/unarchive"), Method.POST);
    }
    public class BexioApiProjectTypeEndpoint : BexioApiFullEndpoint<BexioProjectType>, IBexioApiProjectTypeEndpoint
    {
        public BexioApiProjectTypeEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "pr_project_type") { }
    }
    public class BexioApiProjectStatusEndpoint : BexioApiFullEndpoint<BexioProjectStatus>, IBexioApiProjectStatusEndpoint
    {
        public BexioApiProjectStatusEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "pr_project_state") { }
    }
    public class BexioApiTimesheetEndpoint : BexioApiCrudEndpoint<BexioTimesheet>, IBexioApiTimesheetEndpoint
    {
        public BexioApiTimesheetEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "timesheet") { }
    }
    public class BexioApiTimesheetStatusEndpoint : BexioApiFullEndpoint<BexioTimesheetStatus>, IBexioApiTimesheetStatusEndpoint
    {
        public BexioApiTimesheetStatusEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "timesheet_status") { }
    }
    public class BexioApiClientServiceEndpoint : BexioApiCrudEndpoint<BexioClientService>, IBexioApiClientServiceEndpoint
    {
        public BexioApiClientServiceEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "client_service") { }
    }
    public class BexioApiCommunicationKindEndpoint : BexioApiFullEndpoint<BexioCommunicationKind>, IBexioApiCommunicationKindEndpoint
    {
        public BexioApiCommunicationKindEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "communication_kind") { }
    }
    public class BexioApiNoteEndpoint : BexioApiCrudEndpoint<BexioNote>, IBexioApiNoteEndpoint
    {
        public BexioApiNoteEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "note") { }
    }
    public class BexioApiTaskEndpoint : BexioApiCrudEndpoint<BexioTask>, IBexioApiTaskEndpoint
    {
        public BexioApiTaskEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "task") { }
    }

    // Accounting
    public class BexioApiAccountEndpoint : BexioApiFullEndpoint<BexioAccount>, IBexioApiAccountEndpoint
    {
        public BexioApiAccountEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "accounts") { }
    }
    public class BexioApiAccountGroupEndpoint : BexioApiFullEndpoint<BexioAccountGroup>, IBexioApiAccountGroupEndpoint
    {
        public BexioApiAccountGroupEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "account_groups") { }
    }

    // Other
    public class BexioApiCountryEndpoint : BexioApiCrudEndpoint<BexioCountry>, IBexioApiCountryEndpoint
    {
        public BexioApiCountryEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "country") { }
    }
    public class BexioApiCurrencyEndpoint : BexioApiFullEndpoint<BexioCurrency>, IBexioApiCurrencyEndpoint
    {
        public BexioApiCurrencyEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "currency") { }
    }
    public class BexioApiLanguageEndpoint : BexioApiCrudEndpoint<BexioLanguage>, IBexioApiLanguageEndpoint
    {
        public BexioApiLanguageEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "language") { }
    }
    public class BexioApiPaymentTypeEndpoint : BexioApiFullEndpoint<BexioPaymentType>, IBexioApiPaymentTypeEndpoint
    {
        public BexioApiPaymentTypeEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "payment_type") { }
    }
    public class BexioApiUserEndpoint : BexioApiFullEndpoint<BexioUser>, IBexioApiUserEndpoint
    {
        public BexioApiUserEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "user") { }
    }
    public class BexioApiFictionalUserEndpoint : BexioApiCrudEndpoint<BexioFictionalUser>, IBexioApiFictionalUserEndpoint
    {
        public BexioApiFictionalUserEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "fictional_user") { }
    }
    public class BexioApiCompanyProfileEndpoint : BexioApiFullEndpoint<BexioCompanyProfile>, IBexioApiCompanyProfileEndpoint
    {
        public BexioApiCompanyProfileEndpoint(IBexioApi api) : base(api, BexioApiVersion.V2, "company_profile") { }
    }
}
