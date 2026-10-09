using bexio_lib.Implementation;

namespace bexio_lib.OAuth
{
    public static class BexioOAuthExtensions
    {
        /// <summary>
        /// Client that authenticates with the tokens of one connection (company / user), e.g. one per customer in a multi tenant app:
        /// <code>factory.Create(new BexioOAuthTokenProvider(oauth, storeOfThisCustomer))</code>
        /// </summary>
        public static IBexioClient Create(this IBexioClientFactory factory, IBexioTokenProvider tokenProvider)
            => factory.Create(new BexioOptions { AccessTokenProvider = tokenProvider.GetAccessTokenAsync });
    }
}
