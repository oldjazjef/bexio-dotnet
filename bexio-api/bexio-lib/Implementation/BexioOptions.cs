using System;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    /// <summary>
    /// Settings of a bexio client. Bind from configuration section "Bexio" or configure in code.
    /// </summary>
    public class BexioOptions
    {
        public const string SectionName = "Bexio";

        /// <summary>Api base url without version. Defaults to https://api.bexio.com</summary>
        public string BaseUrl { get; set; } = BexioApi.DEFAULT_API_URL;

        /// <summary>Static bearer token (personal access token or OAuth2 access token)</summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Optional callback to get the token per request, e.g. to refresh OAuth2 tokens.
        /// Takes precedence over <see cref="AccessToken"/>.
        /// </summary>
        public Func<CancellationToken, Task<string>> AccessTokenProvider { get; set; }

        /// <summary>How often a request is repeated after a 429 (rate limit) / 503 response</summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>First retry delay, doubled for every further attempt. A Retry-After header wins.</summary>
        public TimeSpan RetryBaseDelay { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>Upper bound for a single retry delay</summary>
        public TimeSpan RetryMaxDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>Timeout of the HttpClient created by <see cref="BexioClientFactory"/></summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);
    }
}
