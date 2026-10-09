using bexio_lib.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    /// <summary>
    /// HttpClient based transport. Adds the bearer token, builds the url and retries rate limited requests.
    /// </summary>
    public class BexioApi : IBexioApi
    {
        public const string DEFAULT_API_URL = "https://api.bexio.com";

        /// <summary>Name of the HttpClient registered by AddBexio</summary>
        public const string HttpClientName = "bexio";

        private readonly HttpClient _http;
        private readonly BexioOptions _options;
        private readonly Uri _baseUri;

        public string API_URL { get; }

        public BexioApi(HttpClient http, IOptions<BexioOptions> options) : this(http, options.Value) { }

        public BexioApi(HttpClient http, BexioOptions options)
        {
            this._http = http ?? throw new ArgumentNullException(nameof(http));
            this._options = options ?? throw new ArgumentNullException(nameof(options));
            this.API_URL = NormalizeBaseUrl(options.BaseUrl);
            this._baseUri = new Uri(this.API_URL + "/");
        }

        /// <summary>
        /// Convenience for setups without DI: creates an api with its own HttpClient.
        /// Prefer <see cref="BexioClientFactory"/> or AddBexio, which share the HttpClient.
        /// </summary>
        public static BexioApi UseJwt(string apiUrl, string apiKey)
            => new BexioApi(new HttpClient(), new BexioOptions { BaseUrl = apiUrl, AccessToken = apiKey });

        /// <summary>
        /// Removes trailing slashes and a trailing version segment (e.g. /2.0, /3.0) from the url.
        /// </summary>
        public static string NormalizeBaseUrl(string apiUrl)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                return DEFAULT_API_URL;
            }
            var url = apiUrl.Trim().TrimEnd('/');
            return Regex.Replace(url, @"/\d+\.\d+$", string.Empty);
        }

        public BexioResponse Send(BexioRequest request, HttpMethod method)
            => this.SendAsync(request, method).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<BexioResponse> SendAsync(BexioRequest request, HttpMethod method, CancellationToken cancellationToken = default)
        {
            var token = await this.GetTokenAsync(cancellationToken).ConfigureAwait(false);

            for (var attempt = 0; ; attempt++)
            {
                BexioResponse result;
                TimeSpan? retryAfter = null;

                using (var message = this.BuildMessage(request, method, token))
                {
                    try
                    {
                        using var response = await this._http.SendAsync(message, cancellationToken).ConfigureAwait(false);
                        var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        result = new BexioResponse
                        {
                            StatusCode = response.StatusCode,
                            RawBytes = bytes,
                            Content = bytes.Length == 0 ? string.Empty : Encoding.UTF8.GetString(bytes)
                        };
                        retryAfter = response.Headers.RetryAfter?.Delta
                            ?? (response.Headers.RetryAfter?.Date - DateTimeOffset.UtcNow);
                    }
                    catch (HttpRequestException ex)
                    {
                        // no response at all: do not retry blindly, a POST may have been processed
                        return new BexioResponse { ErrorMessage = ex.Message };
                    }
                }

                if (attempt >= this._options.MaxRetries || !IsRetryable(result.StatusCode, method))
                {
                    return result;
                }

                var delay = retryAfter ?? TimeSpan.FromTicks(this._options.RetryBaseDelay.Ticks << attempt);
                if (delay > this._options.RetryMaxDelay) delay = this._options.RetryMaxDelay;
                if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }

        private static bool IsRetryable(HttpStatusCode status, HttpMethod method)
        {
            // 429: request was not processed. 503: not processed either. 502 / 504 only for reads.
            if (status == (HttpStatusCode)429 || status == HttpStatusCode.ServiceUnavailable) return true;
            return method == HttpMethod.Get && (status == HttpStatusCode.BadGateway || status == HttpStatusCode.GatewayTimeout);
        }

        private async Task<string> GetTokenAsync(CancellationToken ct)
        {
            if (this._options.AccessTokenProvider != null)
            {
                return await this._options.AccessTokenProvider(ct).ConfigureAwait(false);
            }
            return this._options.AccessToken;
        }

        private HttpRequestMessage BuildMessage(BexioRequest request, HttpMethod method, string token)
        {
            var relative = request.Resource.TrimStart('/');
            if (request.Query.Count > 0)
            {
                relative += "?" + string.Join("&", request.Query.Select(q => $"{Uri.EscapeDataString(q.Key)}={Uri.EscapeDataString(q.Value ?? string.Empty)}"));
            }

            var message = new HttpRequestMessage(method, new Uri(this._baseUri, relative));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(request.Accept == "*/*" ? "*/*" : request.Accept));

            if (request.Upload != null)
            {
                var form = new MultipartFormDataContent();
                var file = new ByteArrayContent(request.Upload.Content);
                file.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                form.Add(file, request.Upload.FieldName, request.Upload.FileName);
                message.Content = form;
            }
            else if (request.JsonBody != null)
            {
                message.Content = new StringContent(request.JsonBody, Encoding.UTF8, "application/json");
            }
            return message;
        }
    }
}
