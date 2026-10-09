using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.OAuth
{
    /// <summary>Talks to the bexio identity provider</summary>
    public interface IBexioOAuthClient
    {
        /// <summary>Url the user has to be redirected to, to grant the app access</summary>
        /// <param name="state">Random value, verify it again on the redirect (CSRF protection)</param>
        /// <param name="scopes">Overrides the scopes of the options</param>
        string GetAuthorizationUrl(string state, IEnumerable<string> scopes = null);

        /// <summary>Exchanges the code received on the redirect uri for tokens</summary>
        Task<BexioOAuthToken> ExchangeCodeAsync(string code, CancellationToken cancellationToken = default);

        Task<BexioOAuthToken> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Persists the tokens of one bexio connection (one company / user). Implement it with your database;
    /// the default only keeps them in memory.
    /// </summary>
    public interface IBexioTokenStore
    {
        Task<BexioOAuthToken> GetAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(BexioOAuthToken token, CancellationToken cancellationToken = default);
    }

    /// <summary>Delivers valid access tokens, refreshing them when needed</summary>
    public interface IBexioTokenProvider
    {
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    }
}
