# Usage

## Reading

```csharp
var contact  = await client.V2.Contacts.GetByIdAsync(42);
var contacts = await client.V2.Contacts.GetAllAsync(new BexioRequestFilter { limit = 100, offset = 0, order_by = new[] { "id_desc" } });
```

`BexioRequestFilter.limit`, `offset` and `order_by` map to the query parameters of the same name. Bexio returns 500 entries by default.

### Paging

```csharp
await foreach (var contact in client.V2.Contacts.GetAllPagesAsync(pageSize: 500))
{
    ...
}
```

`GetAllPagesAsync` reads page by page until a page is not full.

## Searching

`Search` posts filter instructions to the `search` endpoint of the resource:

```csharp
var filter = new BexioRequestFilter { limit = 50 }
    .Add(new BexioRequestFilterInstruction(BexioInvoiceFilterFields.kb_item_status_id, "9,16", BexioFilterCriteria.IN))
    .Add(new BexioRequestFilterInstruction(BexioInvoiceFilterFields.is_valid_from, "2026-01-01", BexioFilterCriteria.GREATER_EQUAL));

var invoices = await client.V2.Invoices.SearchAsync(filter);
```

- Criteria: `BexioFilterCriteria` (`EXACT_MATCH`, `LIKE`, `IN`, `GREATER_THAN`, ...).
- Field names: `BexioInvoiceFilterFields`, `BexioOrderFilterFields`, `BexioItemFilterFields`, ...
- Status ids: `BexioInvoiceStatus`, `BexioOrderStatus`, `BexioQuoteStatus`.

## Create, update, delete

Endpoints with write access (see [Endpoint reference](endpoints.md)):

```csharp
var created = await client.V2.Contacts.CreateAsync(new BexioContact
{
    contact_type_id = BexioContactTypes.COMPANY,
    name_1 = "Muster AG",
    user_id = 1,
    owner_id = 1
});

await client.V2.Contacts.UpdateAsync(created.id.Value, new BexioContact { name_1 = "Muster GmbH" });
bool deleted = await client.V2.Contacts.DeleteAsync(created.id.Value);
```

Properties that are `null` are **not sent**, so an update only changes what you set. Value types without `?` (for example `bool`) are always sent.

## Sales documents and actions

```csharp
client.V2.Invoices.Issue(id);
client.V2.Invoices.Send(id, new BexioInvoiceSend { recipient_email = "a@b.ch", subject = "Invoice", message = "..." });
BexioPdf pdf = client.V2.Invoices.GetPdf(id);       // base64 decoded into pdf.content

var order   = client.V2.Offers.CreateOrder(offerId);
var invoice = client.V2.Orders.CreateInvoice(orderId, new BexioOrderInvoiceUpdate());
```

Action methods (`Issue`, `Revoke`, `Cancel`, `MarkSent`, `Send`, `GetPdf`, conversions) are synchronous; wrap them in `Task.Run` if you need them off the calling thread.

Positions of a document:

```csharp
var positions = client.V2.InvoicePositions.GetAll(invoiceId, BexioPositionType.KbPositionCustom);
client.V2.InvoicePositions.Create(invoiceId, BexioPositionType.KbPositionCustom, new BexioPosition { text = "Work", amount = 2, unit_price = 100 });
```

## Files (api 3.0)

```csharp
BexioFile file = await client.V3.Files.UploadAsync("contract.pdf", bytes);
byte[] content = await client.V3.Files.DownloadAsync(file.id.Value);
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
