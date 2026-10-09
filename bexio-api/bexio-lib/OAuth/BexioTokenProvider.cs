using System;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.OAuth
{
    /// <summary>Keeps the token in memory only. Fine for tests and single process tools, not for production apps.</summary>
    public class InMemoryBexioTokenStore : IBexioTokenStore
    {
        private BexioOAuthToken _token;

        public InMemoryBexioTokenStore(BexioOAuthToken token = null)
        {
            this._token = token;
        }

        public Task<BexioOAuthToken> GetAsync(CancellationToken cancellationToken = default) => Task.FromResult(this._token);

        public Task SaveAsync(BexioOAuthToken token, CancellationToken cancellationToken = default)
        {
            this._token = token;
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Returns the stored access token and transparently refreshes it shortly before it expires.
    /// Concurrent callers share one refresh. The (possibly rotated) refresh token is saved to the store.
    /// </summary>
    public class BexioOAuthTokenProvider : IBexioTokenProvider
    {
        private readonly IBexioOAuthClient _oauth;
        private readonly IBexioTokenStore _store;
        private readonly TimeSpan _skew;
        private readonly TimeProvider _time;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public BexioOAuthTokenProvider(IBexioOAuthClient oauth, IBexioTokenStore store, BexioOAuthOptions options = null, TimeProvider time = null)
        {
            this._oauth = oauth ?? throw new ArgumentNullException(nameof(oauth));
            this._store = store ?? throw new ArgumentNullException(nameof(store));
            this._skew = (options ?? new BexioOAuthOptions()).RefreshSkew;
            this._time = time ?? TimeProvider.System;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            var token = await this._store.GetAsync(cancellationToken).ConfigureAwait(false);
            if (token == null)
            {
                throw new InvalidOperationException("No bexio token available. Let the user authorize the app first (IBexioOAuthClient.ExchangeCodeAsync) and save the token to the store.");
            }
            if (!token.IsExpired(this._time.GetUtcNow(), this._skew))
            {
                return token.AccessToken;
            }

            await this._lock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                // another caller may have refreshed meanwhile
                token = await this._store.GetAsync(cancellationToken).ConfigureAwait(false);
                if (!token.IsExpired(this._time.GetUtcNow(), this._skew))
                {
                    return token.AccessToken;
                }
                if (string.IsNullOrEmpty(token.RefreshToken))
                {
                    throw new InvalidOperationException("The bexio access token expired and there is no refresh token. Request the scope offline_access and authorize again.");
                }

                var refreshed = await this._oauth.RefreshAsync(token.RefreshToken, cancellationToken).ConfigureAwait(false);
                // keep the old refresh token if the server did not send a new one
                refreshed.RefreshToken ??= token.RefreshToken;
                await this._store.SaveAsync(refreshed, cancellationToken).ConfigureAwait(false);
                return refreshed.AccessToken;
            }
            finally
            {
                this._lock.Release();
            }
        }
    }
}
