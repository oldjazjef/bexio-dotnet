using System;

namespace bexio_lib.OAuth
{
    public class BexioOAuthToken
    {
        public string AccessToken { get; set; }

        /// <summary>Needed to get new access tokens. Bexio may rotate it on every refresh, always store the latest.</summary>
        public string RefreshToken { get; set; }

        public string IdToken { get; set; }
        public string Scope { get; set; }

        /// <summary>UTC time when the access token expires</summary>
        public DateTimeOffset ExpiresAt { get; set; }

        public bool IsExpired(DateTimeOffset now, TimeSpan skew) => now + skew >= this.ExpiresAt;
    }
}
