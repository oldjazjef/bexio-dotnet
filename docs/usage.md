# Usage

## Reading

```csharp
var contact  = await client.V2.Contacts.GetByIdAsync(42);
var contacts = await client.V2.Contacts.GetAllAsync(new BexioRequestFilter { limit = 100, offset = 0, order_by = new[] { "id_desc" } });
```

`BexioRequestFilter.limit`, `offset` and `order_by` map to the query parameters of the same name. Bexio returns 500 entries by default.

### Paging

```csharp
await foreach (var contact in client.V2.Contacts.ListContactsPagesAsync(pageSize: 500))
{
    ...
}
```

Every list and search operation has a `...PagesAsync` variant (`ListContactsPagesAsync`, `SearchInvoicesPagesAsync`, ...). It reads page by page until a page is not full.

## Searching

`Search` posts filter instructions to the `search` endpoint of the resource:

```csharp
var filter = new BexioRequestFilter { limit = 50 }
    .Add(new BexioRequestFilterInstruction(BexioInvoiceFilterFields.kb_item_status_id, "9,16", BexioFilterCriteria.IN))
    .Add(new BexioRequestFilterInstruction(BexioInvoiceFilterFields.is_valid_from, "2026-01-01", BexioFilterCriteria.GREATER_EQUAL));

var invoices = await client.V2.Invoices.SearchAsync(filter);
```

- Criteria: `BexioFilterCriteria` (`EXACT_MATCH`, `LIKE`, `IN`, `GREATER_THAN`, ...).
- Field names: `BexioSearchFields.KbInvoice`, `BexioSearchFields.Contact`, ... (generated from the bexio documentation, one class per search endpoint)
- Status ids: `BexioInvoiceStatus`, `BexioOrderStatus`, `BexioQuoteStatus`.

## Create, update, delete

Endpoints with write access (see [Endpoint reference](endpoints.md)):

```csharp
var created = await client.V2.Contacts.CreateAsync(new BexioContactRequest
{
    contact_type_id = BexioContactTypes.COMPANY,
    name_1 = "Muster AG",
    user_id = 1,
    owner_id = 1
});

await client.V2.Contacts.UpdateAsync(created.id.Value, new BexioContactRequest { name_1 = "Muster GmbH" });
bool deleted = await client.V2.Contacts.DeleteAsync(created.id.Value);
```

Properties that are `null` are **not sent**, so an update only changes what you set. Value types without `?` (for example `bool`) are always sent.

## Sales documents and actions

```csharp
client.V2.Invoices.IssueInvoice(id);
client.V2.Invoices.SendInvoice(id, new BexioNetworkSendRequest { recipient_email = "a@b.ch", subject = "Invoice", message = "..." });
BexioDocumentPDF pdf = client.V2.Invoices.ShowInvoicePDF(id, logopaper: 1);    // content is base64

var order   = client.V2.Quotes.CreateOrderFromQuote(quoteId, new BexioKbCreateFromDocumentRequest());
var invoice = client.V2.Orders.CreateInvoiceFromOrder(orderId, new BexioKbCreateFromDocumentRequest());
```

Actions return `bool` (the `success` flag of bexio) or the created document. Every action is also available as `...Async`.

Positions and comments of a document. The document type is `kb_offer`, `kb_order` or `kb_invoice`:

```csharp
var positions = client.V2.DefaultPositions.ListDefaultPositions("kb_invoice", invoiceId);
client.V2.DefaultPositions.CreateDefaultPosition("kb_invoice", invoiceId, new BexioPositionCustom { text = "Work", amount = "2", unit_price = "100" });
```

## Files (api 3.0)

```csharp
var files = await client.V3.Files.CreateFileAsync("contract.pdf", bytes);
byte[] content = await client.V3.Files.DownloadFileAsync(files.First().id.Value);
```

## Errors

Every non 2xx answer throws `BexioApiException`:

```csharp
try { await client.V2.Contacts.GetByIdAsync(1); }
catch (BexioApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound) { ... }
```

`ex.Message` contains the bexio message (and validation `errors` if present), `ex.Content` the raw body. A request that gets no answer at all (network error) throws with the transport error message and no status code.

## Retries and rate limits

Bexio limits the number of requests. The library retries on:

- `429` and `503` for every method (the request was not processed),
- `502` and `504` for `GET` only.

The delay is the `Retry-After` header if present, otherwise `RetryBaseDelay` doubled per attempt, capped by `RetryMaxDelay`. After `MaxRetries` the last answer is returned and raised as `BexioApiException`. Network errors are not retried (a `POST` may already have been processed).

## Extending with your own endpoint

```csharp
public interface IBexioApiMyThingEndpoint : IBexioApiCrudEndpoint<MyThing> { }

public class BexioApiMyThingEndpoint : BexioApiCrudEndpoint<MyThing>, IBexioApiMyThingEndpoint
{
    public BexioApiMyThingEndpoint(IBexioApi api) : base(api, BexioApiVersion.V3, "my_things") { }
}
```

`AddBexio` registers every class in the library assembly. For endpoints in your own assembly register them yourself: `services.AddTransient<IBexioApiMyThingEndpoint, BexioApiMyThingEndpoint>()`. Use `Send<T>` / `SendAsync<T>` with a `BexioRequest` for special calls.
