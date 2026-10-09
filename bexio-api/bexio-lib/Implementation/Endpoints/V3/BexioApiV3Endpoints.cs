using bexio_lib.Data;
using bexio_lib.Interfaces;
using bexio_lib.Interfaces.V3;
using bexio_lib.Models;
using RestSharp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints.V3
{
    /// <summary>
    /// Base for api 3.0 resources, which edit with PUT instead of POST.
    /// </summary>
    public abstract class BexioApiV3CrudEndpoint<TEntity> : BexioApiCrudEndpoint<TEntity>
    {
        protected BexioApiV3CrudEndpoint(IBexioApi api, string endpoint) : base(api, BexioApiVersion.V3, endpoint) { }

        protected override Method UpdateMethod => Method.PUT;
    }

    public class BexioApiCurrencyV3Endpoint : BexioApiV3CrudEndpoint<BexioCurrency>, IBexioApiCurrencyV3Endpoint
    {
        public BexioApiCurrencyV3Endpoint(IBexioApi api) : base(api, "currencies") { }

        public ICollection<BexioExchangeRate> GetExchangeRates(int currencyId)
            => this.Send<ICollection<BexioExchangeRate>>(this.NewRequest($"{currencyId}/exchange_rates"), Method.GET);
    }

    public class BexioApiTaxEndpoint : BexioApiFullEndpoint<BexioTax>, IBexioApiTaxEndpoint
    {
        public BexioApiTaxEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "taxes") { }

        public bool Delete(int id)
            => this.API.Execute(this.NewRequest(id.ToString()), Method.DELETE).ToDeleteResult();
    }

    public class BexioApiUserV3Endpoint : BexioApiFullEndpoint<BexioUser>, IBexioApiUserV3Endpoint
    {
        public BexioApiUserV3Endpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "users") { }

        public BexioUser GetMe() => this.Send<BexioUser>(this.NewRequest("me"), Method.GET);
    }

    public class BexioApiCalendarYearEndpoint : BexioApiFullEndpoint<BexioCalendarYear>, IBexioApiCalendarYearEndpoint
    {
        public BexioApiCalendarYearEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "accounting/calendar_years") { }

        public BexioCalendarYear Create(BexioCalendarYear year)
            => this.Send<BexioCalendarYear>(this.NewRequest().AddRequestBodyData(year), Method.POST);
    }

    public class BexioApiBusinessYearEndpoint : BexioApiFullEndpoint<BexioBusinessYear>, IBexioApiBusinessYearEndpoint
    {
        public BexioApiBusinessYearEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "accounting/business_years") { }
    }

    public class BexioApiVatPeriodEndpoint : BexioApiFullEndpoint<BexioVatPeriod>, IBexioApiVatPeriodEndpoint
    {
        public BexioApiVatPeriodEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "accounting/vat_periods") { }
    }

    public class BexioApiManualEntryEndpoint : BexioApiV3CrudEndpoint<BexioManualEntry>, IBexioApiManualEntryEndpoint
    {
        public BexioApiManualEntryEndpoint(IBexioApi api) : base(api, "accounting/manual_entries") { }

        public string GetNextReferenceNumber()
            => this.Send<BexioNextReferenceNumber>(this.NewRequest("next_ref_nr"), Method.GET)?.next_ref_nr;
    }

    public class BexioApiJournalEndpoint : BexioApiEndpoint, IBexioApiJournalEndpoint
    {
        public BexioApiJournalEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "accounting/journal") { }

        private RestRequest Req(string from, string to, BexioRequestFilter requestParameter)
        {
            var request = this.NewRequest().AddRequestData(requestParameter);
            if (!string.IsNullOrEmpty(from))
            {
                request.AddQueryParameter("from", from);
            }
            if (!string.IsNullOrEmpty(to))
            {
                request.AddQueryParameter("to", to);
            }
            return request;
        }

        public ICollection<BexioJournalEntry> GetAll(string from = null, string to = null, BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<BexioJournalEntry>>(this.Req(from, to, requestParameter), Method.GET);

        public Task<ICollection<BexioJournalEntry>> GetAllAsync(string from = null, string to = null, BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<BexioJournalEntry>>(this.Req(from, to, requestParameter), Method.GET, cancellationToken);
    }

    public class BexioApiBankAccountEndpoint : BexioApiFullEndpoint<BexioBankAccount>, IBexioApiBankAccountEndpoint
    {
        public BexioApiBankAccountEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "banking/accounts") { }
    }

    public class BexioApiFileEndpoint : BexioApiFullEndpoint<BexioFile>, IBexioApiFileEndpoint
    {
        public BexioApiFileEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "files") { }

        private RestRequest UploadRequest(string fileName, byte[] content)
        {
            var request = this.NewRequest();
            request.AlwaysMultipartFormData = true;
            request.AddFile("file", content, fileName);
            return request;
        }

        public BexioFile Upload(string fileName, byte[] content)
        {
            var files = this.Send<ICollection<BexioFile>>(this.UploadRequest(fileName, content), Method.POST);
            return files == null ? null : System.Linq.Enumerable.FirstOrDefault(files);
        }

        public async Task<BexioFile> UploadAsync(string fileName, byte[] content, CancellationToken cancellationToken = default)
        {
            var files = await this.SendAsync<ICollection<BexioFile>>(this.UploadRequest(fileName, content), Method.POST, cancellationToken).ConfigureAwait(false);
            return files == null ? null : System.Linq.Enumerable.FirstOrDefault(files);
        }

        private RestRequest DownloadRequest(int fileId)
        {
            var request = this.NewRequest($"{fileId}/download");
            request.AddHeader("Accept", "*/*");
            return request;
        }

        public byte[] Download(int fileId)
            => this.API.Execute(this.DownloadRequest(fileId), Method.GET).EnsureSuccess().RawBytes;

        public async Task<byte[]> DownloadAsync(int fileId, CancellationToken cancellationToken = default)
            => (await this.API.ExecuteAsync(this.DownloadRequest(fileId), Method.GET, cancellationToken).ConfigureAwait(false)).EnsureSuccess().RawBytes;

        public bool Delete(int fileId)
            => this.API.Execute(this.NewRequest(fileId.ToString()), Method.DELETE).ToDeleteResult();
    }
}
