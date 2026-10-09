# dotnet core Bexio client

![bexio-dotnet](docs/images/bexio-dotnet.webp)

This repository contains a dotnet / dotnet core compatible Bexio client.

Supports **every operation of the bexio API 2.0 and 3.0** (272 operations). Endpoints and models are generated from the official OpenAPI description of [docs.bexio.com](https://docs.bexio.com/), so the client covers what bexio documents and follows it when it changes. Every endpoint knows its own version, so both can be used side by side with one access token. All calls exist as sync and async (`...Async`) methods, targets .NET 10 (LTS).

#### API 2.0 (`client.V2`)
- **Contacts:** contacts (incl. restore and bulk create), relations, groups, sectors, additional addresses, salutations, titles
- **Sales:** quotes, orders (incl. repetitions), invoices (incl. payments, reminders, copy, cancel, send, pdf), delivery notes, conversions between documents
- **Document positions and comments:** all position types (item, custom, text, subtotal, sub position, discount, page break) and comments of quotes, orders and invoices
- **Items and stock:** items, stock locations and areas
- **Projects and time tracking:** projects (archive / reactivate), timesheets, business activities, notes, tasks
- **Master data:** accounts, account groups, countries, languages, communication types, payment types, units, company profile, document settings

#### API 3.0 (`client.V3`)
- **Accounting:** calendar years, business years, VAT periods, manual entries (incl. files and next reference number), journal
- **Banking and money:** bank accounts, currencies (incl. exchange rates), taxes
- **Files:** upload, download, preview, search, usage
- **Projects:** milestones and work packages
- **Purchase:** purchase orders
- **Users:** users, fictional users, permissions, document templates, reports

All operations with their methods and scopes: [docs/endpoints.md](docs/endpoints.md). Method names are the operation ids of the bexio documentation (`ListContacts`, `ShowContact`, `CreateContact`, `EditContact`, ...). Groups with a main collection additionally offer the short forms `GetAll`, `GetById`, `Search`, `Create`, `Update` and `Delete`, and list / search operations offer `...PagesAsync` to read all pages.

> bexio also has an API 4.0 (contacts v2, purchase, payroll, ...). It is not part of this library yet.

If you find something missing or broken, please [report an issue][github-issue] or even better fork the repo and submit a pull request


### How to use

#### Setup

**With dependency injection** (recommended). Registers a typed `HttpClient` (`IHttpClientFactory`), the client, the factory and every single endpoint:

```csharp
// appsettings.json: "Bexio": { "AccessToken": "...", "BaseUrl": "https://api.bexio.com" }
services.AddBexio(Configuration.GetSection("Bexio"));

// or in code
services.AddBexio(o => o.AccessToken = "myApiKey");
```

Options (`BexioOptions`): `AccessToken` (personal access token), `AccessTokenProvider` (async callback, e.g. to refresh OAuth2 tokens), `BaseUrl`, `MaxRetries`, `RetryBaseDelay`, `RetryMaxDelay`, `Timeout`.
The old `services.AddBexioJwt(configuration)` (keys `bexioApiKey` / `bexioApiUrl`) still works.

**Without dependency injection / several bexio accounts**: use the factory

```csharp
var factory = new BexioClientFactory();
var client = factory.Create("myApiKey");     // IBexioClient
```

Requests answered with 429 (rate limit) or 503 are retried automatically (honors `Retry-After`, exponential backoff otherwise).

#### Authentication: personal access token or OAuth2

Two ways to authenticate against bexio are supported:

| | Personal access token (PAT) | OAuth2 (app registered at bexio) |
|---|---|---|
| For | own scripts / internal tools | apps used with several bexio accounts |
| Setup | `AccessToken` in `BexioOptions` | `AddBexioOAuth` + token store |

##### OAuth2 (authorization code flow)

1. Register your app in the bexio developer portal and note client id / secret, set the redirect uri.
2. Register the services. The OAuth tokens are fetched, refreshed shortly before they expire and handed to every API request automatically:

```csharp
services.AddBexio(_ => { });                       // no static token needed
services.AddBexioOAuth(o =>
{
    o.ClientId = "...";
    o.ClientSecret = "...";
    o.RedirectUri = "https://myapp.example/bexio/callback";
    o.Scopes = new[] { "contact_show", "kb_invoice_edit" };   // "openid" and "offline_access" are added for you
});
services.AddSingleton<IBexioTokenStore, MyDatabaseTokenStore>();   // see below
```

Or bind from configuration: `services.AddBexioOAuth(Configuration.GetSection("BexioOAuth"))`.

3. Send the user to bexio and handle the redirect:

```csharp
// 1) redirect the user
var url = _oauth.GetAuthorizationUrl(state);        // IBexioOAuthClient, "state": random value, verify it on the callback
return Redirect(url);

// 2) on your redirect uri
var token = await _oauth.ExchangeCodeAsync(code);
await _tokenStore.SaveAsync(token);
```

From then on `IBexioClient` and all endpoints use (and refresh) that token.

**Token store.** Bexio rotates refresh tokens, so the newest token must be persisted. Implement `IBexioTokenStore` (`GetAsync` / `SaveAsync`) on top of your database. The default `InMemoryBexioTokenStore` loses tokens on restart and is only meant for tests. The token provider is a singleton, so register the store as singleton as well (use `IServiceScopeFactory` inside it if it needs scoped services like a DbContext).

**Several bexio accounts (multi tenant).** Create one store per connection and build a client per customer:

```csharp
var provider = new BexioOAuthTokenProvider(oauthClient, storeOfThisCustomer, oauthOptions);
IBexioClient client = factory.Create(provider);     // IBexioClientFactory
```

Without DI: `new BexioOAuthClient(httpClient, new BexioOAuthOptions { ... })`.

#### Consume api

Either inject `IBexioClient` and pick the endpoint by api version, or inject just the endpoint you need:

```csharp
public class LeadsController : ControllerBase
{
    private readonly IBexioClient _bexio;

    public LeadsController(IBexioClient bexio) => _bexio = bexio;

    [HttpPost]
    public async Task<IActionResult> FilterLeads([FromBody] QueryLeadsViewModel vm)
    {
        var invoices = await _bexio.V2.Invoices.SearchAsync(new BexioRequestFilter()
            .Add(new BexioRequestFilterInstruction(
                BexioInvoiceFilterFields.kb_item_status_id,
                $"{BexioInvoiceStatus.PAID},{BexioInvoiceStatus.PENDING},{BexioInvoiceStatus.PARTIAL}",
                BexioFilterCriteria.IN))
            .Add(new BexioRequestFilterInstruction(
                BexioInvoiceFilterFields.is_valid_from,
                vm.FromDate.ToString("o", CultureInfo.InvariantCulture),
                BexioFilterCriteria.GREATER_THAN)));
        return Ok(invoices);
    }
}
```

`IBexioApiInvoiceEndpoint` etc. can still be injected directly.

#### Async, v3, CRUD, paging

```csharp
var currencies = await _bexio.V3.Currencies.ListCurrenciesAsync();
var contact = await _bexio.V2.Contacts.CreateAsync(new BexioContactRequest { name_1 = "Muster AG", contact_type_id = BexioContactTypes.COMPANY, user_id = 1, owner_id = 1 });

// every contact, page by page
await foreach (var c in _bexio.V2.Contacts.ListContactsPagesAsync()) { ... }
```

Failed requests throw a `BexioApiException` with `StatusCode` and the raw `Content`.

### Tests, CI and release
`dotnet test bexio-api/BexioLibTest` runs unit tests against a fake api. Integration tests run only if `bexioApiKey` is set as environment variable.
Transport behaviour (auth header, url, retry, multipart, paging) is tested against a stubbed `HttpMessageHandler`.

- **CI** (`.github/workflows/ci.yml`): build with warnings as errors, tests with coverage, NuGet pack on every push / pull request.
- **Release** (`.github/workflows/release.yml`): push a tag `v1.2.3` and the package `Bexio.DotNet` is tested, packed with that version, pushed to nuget.org and attached to a GitHub release. Publishing uses [NuGet trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing) (OIDC, no API key stored): add a trusted publishing policy on nuget.org for this repository, `release.yml` and the GitHub environment `production`, and set the repository variable `NUGET_USER` to your nuget.org profile name.
- Dependabot keeps NuGet packages and actions up to date.

### Upgrading from 2.x

3.0 replaces the hand written endpoints by generated ones. `GetAll`, `GetById`, `Search`, `Create`, `Update` and `Delete` still exist on the main collection of each group, but the groups, models and action methods are named after the bexio documentation:

| 2.x | 3.0 |
|---|---|
| `IBexioApiInvoiceEndpoint`, `client.V2.Invoices` | `IBexioApiInvoicesEndpoint`, `client.V2.Invoices` |
| `Issue(id)`, `Send(id, ...)`, `MarkSent(id)` | `IssueInvoice(id)`, `SendInvoice(id, ...)`, `MarkAsSentInvoice(id)` |
| `Revoke(id)` | `RevertIssueInvoice(id)` |
| `GetPdf(id)` | `ShowInvoicePDF(id, logopaper)` |
| `BexioContact` (create / edit) | `BexioContactRequest` |
| `client.V2.Offers` | `client.V2.Quotes` |
| `client.V2.Articles` | `client.V2.Items` |
| `GetAllPagesAsync()` | `ListContactsPagesAsync()` (per operation) |
| `client.V4` | removed, API 4.0 is not part of the library |

### Documentation

Full documentation is in [`docs/`](docs/index.md):

- [Getting started](docs/getting-started.md): install, setup with or without dependency injection
- [Authentication](docs/authentication.md): personal access token and OAuth2, token store, several bexio accounts
- [Usage](docs/usage.md): filters and search, paging, create / update / delete, errors, retries, own endpoints
- [Endpoint reference](docs/endpoints.md): all endpoints of api 2.0 and 3.0
- [Testing](docs/testing.md) and [Releasing](docs/releasing.md)

The package also ships XML documentation for IntelliSense.
