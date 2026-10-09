using bexio_lib.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces.V3
{
    public interface IBexioApiCurrencyV3Endpoint : IBexioApiCrudEndpoint<BexioCurrency>
    {
        ICollection<BexioExchangeRate> GetExchangeRates(int currencyId);
    }

    public interface IBexioApiTaxEndpoint : IBexioApiFullEndpoint<BexioTax>
    {
        bool Delete(int id);
    }

    public interface IBexioApiUserV3Endpoint : IBexioApiFullEndpoint<BexioUser>
    {
        /// <summary>The user the access token belongs to</summary>
        BexioUser GetMe();
    }

    public interface IBexioApiCalendarYearEndpoint : IBexioApiFullEndpoint<BexioCalendarYear>
    {
        BexioCalendarYear Create(BexioCalendarYear year);
    }

    public interface IBexioApiBusinessYearEndpoint : IBexioApiFullEndpoint<BexioBusinessYear> { }

    public interface IBexioApiVatPeriodEndpoint : IBexioApiFullEndpoint<BexioVatPeriod> { }

    public interface IBexioApiManualEntryEndpoint : IBexioApiCrudEndpoint<BexioManualEntry>
    {
        /// <summary>Next free reference number for a manual entry</summary>
        string GetNextReferenceNumber();
    }

    public interface IBexioApiJournalEndpoint : IBexioApiEndpoint
    {
        /// <param name="from">Start date (yyyy-MM-dd)</param>
        /// <param name="to">End date (yyyy-MM-dd)</param>
        /// <param name="requestParameter">Optional limit / offset / order</param>
        ICollection<BexioJournalEntry> GetAll(string from = null, string to = null, BexioRequestFilter requestParameter = null);
        Task<ICollection<BexioJournalEntry>> GetAllAsync(string from = null, string to = null, BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default);
    }

    public interface IBexioApiBankAccountEndpoint : IBexioApiFullEndpoint<BexioBankAccount> { }

    public interface IBexioApiFileEndpoint : IBexioApiFullEndpoint<BexioFile>
    {
        BexioFile Upload(string fileName, byte[] content);
        byte[] Download(int fileId);
        bool Delete(int fileId);

        Task<BexioFile> UploadAsync(string fileName, byte[] content, CancellationToken cancellationToken = default);
        Task<byte[]> DownloadAsync(int fileId, CancellationToken cancellationToken = default);
    }
}
