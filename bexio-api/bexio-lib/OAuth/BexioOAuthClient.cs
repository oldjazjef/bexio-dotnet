using bexio_lib.Implementation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.OAuth
{
    public class BexioOAuthClient : IBexioOAuthClient
    {
        /// <summary>Name of the HttpClient registered by AddBexioOAuth</summary>
        public const string HttpClientName = "bexio-oauth";

        private readonly HttpClient _http;
        private readonly BexioOAuthOptions _options;
        private readonly TimeProvider _time;

        public BexioOAuthClient(HttpClient http, IOptions<BexioOAuthOptions> options, TimeProvider time = null)
            : this(http, options.Value, time) { }

        public BexioOAuthClient(HttpClient http, BexioOAuthOptions options, TimeProvider time = null)
        {
            this._http = http ?? throw new ArgumentNullException(nameof(http));
            this._options = options ?? throw new ArgumentNullException(nameof(options));
            this._time = time ?? TimeProvider.System;
        }

        public string GetAuthorizationUrl(string state, IEnumerable<string> scopes = null)
        {
            if (string.IsNullOrEmpty(this._options.ClientId)) throw new InvalidOperationException("ClientId is required");
            if (string.IsNullOrEmpty(this._options.RedirectUri)) throw new InvalidOperationException("RedirectUri is required");

            var parameters = new List<KeyValuePair<string, string>>
            {
                new("client_id", this._options.ClientId),
                new("redirect_uri", this._options.RedirectUri),
                new("response_type", "code"),
                new("scope", string.Join(" ", this.BuildScopes(scopes))),
            };
            if (!string.IsNullOrEmpty(state))
            {
                parameters.Add(new("state", state));
            }
            var query = string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            return $"{this._options.TrimmedAuthority}/protocol/openid-connect/auth?{query}";
        }

        public Task<BexioOAuthToken> ExchangeCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("code is required", nameof(code));
            return this.RequestTokenAsync(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = this._options.RedirectUri,
            }, cancellationToken);
        }

        public Task<BexioOAuthToken> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentException("refreshToken is required", nameof(refreshToken));
            return this.RequestTokenAsync(new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
            }, cancellationToken);
        }

        private IEnumerable<string> BuildScopes(IEnumerable<string> scopes)
        {
            var all = new List<string> { "openid", "offline_access" };
            all.AddRange(scopes ?? this._options.Scopes ?? Enumerable.Empty<string>());
            return all.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
        }

        private async Task<BexioOAuthToken> RequestTokenAsync(Dictionary<string, string> form, CancellationToken ct)
        {
            form["client_id"] = this._options.ClientId;
            form["client_secret"] = this._options.ClientSecret;

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{this._options.TrimmedAuthority}/protocol/openid-connect/token")
            {
                Content = new FormUrlEncodedContent(form)
            };
            request.Headers.Accept.ParseAdd("application/json");

            using var response = await this._http.SendAsync(request, ct).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new BexioApiException(response.StatusCode, content);
            }

            var json = JObject.Parse(content);
            var accessToken = (string)json["access_token"];
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new BexioApiException(response.StatusCode, "Token response contains no access_token");
            }
            var expiresIn = (int?)json["expires_in"] ?? 300;
            return new BexioOAuthToken
            {
                AccessToken = accessToken,
                RefreshToken = (string)json["refresh_token"],
                IdToken = (string)json["id_token"],
                Scope = (string)json["scope"],
                ExpiresAt = this._time.GetUtcNow().AddSeconds(expiresIn)
            };
        }
    }
}
