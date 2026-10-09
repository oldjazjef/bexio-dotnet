using System;
using System.Collections.Generic;

namespace bexio_lib.OAuth
{
    /// <summary>
    /// Settings of a bexio app registered in the bexio developer portal (OAuth2 / OpenID Connect, authorization code flow).
    /// </summary>
    public class BexioOAuthOptions
    {
        public const string SectionName = "BexioOAuth";
        public const string DefaultAuthority = "https://auth.bexio.com/realms/bexio";

        /// <summary>Identity provider of bexio</summary>
        public string Authority { get; set; } = DefaultAuthority;

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        /// <summary>Redirect uri registered for the app</summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Requested scopes. "openid" and "offline_access" (needed for refresh tokens) are added automatically.
        /// Api scopes are e.g. contact_show, kb_invoice_edit, project_show ...
        /// </summary>
        public ICollection<string> Scopes { get; set; } = new List<string>();

        /// <summary>Tokens are refreshed this long before they expire</summary>
        public TimeSpan RefreshSkew { get; set; } = TimeSpan.FromSeconds(60);

        internal string TrimmedAuthority => (Authority ?? DefaultAuthority).TrimEnd('/');
    }
}
