using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace bexio_lib.Implementation
{
    /// <summary>
    /// Creates clients for different tokens / accounts (multi tenant) without touching global state.
    /// </summary>
    public interface IBexioClientFactory
    {
        /// <summary>Client with the default options, but another token</summary>
        IBexioClient Create(string accessToken);

        /// <summary>Client with fully custom options</summary>
        IBexioClient Create(BexioOptions options);
    }

    public class BexioClientFactory : IBexioClientFactory
    {
        // One HttpClient for all clients created without DI, to avoid socket exhaustion.
        private static readonly Lazy<HttpClient> SharedHttpClient = new Lazy<HttpClient>(() => new HttpClient());

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BexioOptions _defaults;

        /// <summary>Without DI: uses a shared HttpClient</summary>
        public BexioClientFactory() : this(null, new BexioOptions()) { }

        public BexioClientFactory(BexioOptions defaults) : this(null, defaults) { }

        public BexioClientFactory(IHttpClientFactory httpClientFactory, IOptions<BexioOptions> defaults)
            : this(httpClientFactory, defaults.Value) { }

        private BexioClientFactory(IHttpClientFactory httpClientFactory, BexioOptions defaults)
        {
            this._httpClientFactory = httpClientFactory;
            this._defaults = defaults ?? new BexioOptions();
        }

        public IBexioClient Create(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentException("An access token is required", nameof(accessToken));
            return this.Create(new BexioOptions
            {
                BaseUrl = this._defaults.BaseUrl,
                AccessToken = accessToken,
                MaxRetries = this._defaults.MaxRetries,
                RetryBaseDelay = this._defaults.RetryBaseDelay,
                RetryMaxDelay = this._defaults.RetryMaxDelay,
                Timeout = this._defaults.Timeout
            });
        }

        public IBexioClient Create(BexioOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            var http = this._httpClientFactory?.CreateClient(BexioApi.HttpClientName) ?? SharedHttpClient.Value;
            return new BexioClient(new BexioApi(http, options));
        }
    }
}
