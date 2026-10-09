# Authentication

bexio accepts a bearer token. The library supports two ways to get one:

| | Personal access token (PAT) | OAuth2 (app registered at bexio) |
|---|---|---|
| For | own scripts, internal tools | apps that act for several bexio accounts |
| Setup | `AccessToken` | `AddBexioOAuth` + token store |
| Refresh | not needed | automatic |

## Personal access token

Create the token in bexio and pass it as `BexioOptions.AccessToken` (see [Getting started](getting-started.md)).

## OAuth2 (authorization code flow)

bexio uses OpenID Connect at `https://auth.bexio.com/realms/bexio`.

1. Register your app in the bexio developer portal and note client id and secret. Set the redirect uri.
2. Register the services:

```csharp
builder.Services.AddBexio(_ => { });            // no static token needed
builder.Services.AddBexioOAuth(o =>
{
    o.ClientId = "...";
    o.ClientSecret = "...";
    o.RedirectUri = "https://myapp.example/bexio/callback";
    o.Scopes = new[] { "contact_show", "kb_invoice_edit" };
});
builder.Services.AddSingleton<IBexioTokenStore, MyDatabaseTokenStore>();
```

`openid` and `offline_access` (needed for refresh tokens) are added automatically. Bind from configuration with `AddBexioOAuth(config.GetSection("BexioOAuth"))`.

`BexioOAuthOptions`: `ClientId`, `ClientSecret`, `RedirectUri`, `Scopes`, `Authority` (default `https://auth.bexio.com/realms/bexio`), `RefreshSkew` (default 60 s, tokens are refreshed this long before they expire).

3. Send the user to bexio and handle the redirect:

```csharp
// IBexioOAuthClient oauth, IBexioTokenStore tokenStore
// 1) start: remember "state" (e.g. in the session) and redirect
var url = oauth.GetAuthorizationUrl(state);
return Redirect(url);

// 2) on your redirect uri: verify "state", then
var token = await oauth.ExchangeCodeAsync(code);
await tokenStore.SaveAsync(token);
```

From then on `IBexioClient` and every endpoint use the stored token and refresh it when it is about to expire. Concurrent requests share a single refresh.

### Token store

Bexio rotates refresh tokens: after every refresh the **new** refresh token must be saved. Implement `IBexioTokenStore` on your database:

```csharp
public interface IBexioTokenStore
{
    Task<BexioOAuthToken> GetAsync(CancellationToken ct = default);
    Task SaveAsync(BexioOAuthToken token, CancellationToken ct = default);
}
```

- The default `InMemoryBexioTokenStore` forgets tokens on restart, use it for tests only.
- The token provider is a singleton. Register the store as singleton too. If it needs scoped services (e.g. a `DbContext`), resolve them through `IServiceScopeFactory` inside the store.
- Without a refresh token (scope `offline_access` missing) an expired token cannot be renewed, the user has to authorize again. The provider then throws an `InvalidOperationException` that says so.

### Several bexio accounts (multi tenant)

Keep one store per connection and build one client per customer:

```csharp
var provider = new BexioOAuthTokenProvider(oauthClient, storeOfThisCustomer, oauthOptions);
IBexioClient client = clientFactory.Create(provider);   // IBexioClientFactory
```

### Without dependency injection

```csharp
var oauth = new BexioOAuthClient(new HttpClient(), new BexioOAuthOptions { ClientId = "...", ClientSecret = "...", RedirectUri = "..." });
var provider = new BexioOAuthTokenProvider(oauth, new InMemoryBexioTokenStore(token));
var client = new BexioClientFactory().Create(provider);
```

### Errors

Failed token requests throw `BexioApiException` with the status code and the answer of the identity provider (for example `invalid_grant` for an expired or already used code).
