
# dotnet core Bexio client

This repository contains a dotnet / dotnet core compatible Bexio client.

Supports **bexio API v2.0, v3.0 and v4.0**. Every endpoint knows its own version, so both can be used side by side with one api key.
All endpoints offer sync and async (`...Async`) methods, targets .NET 8.

#### API 2.0 (`bexio_lib.Interfaces`)
- Sales: orders (`kb_order`), offers (`kb_offer`), invoices (`kb_invoice`), deliveries (`kb_delivery`) incl. issue / revoke / cancel / send / mark as sent / pdf / convert (offer -> order/invoice, order -> invoice/delivery)
- Invoice payments, document positions (invoice / order / offer)
- Contacts, contact relations / groups / sectors, additional addresses, salutations, titles
- Articles, article types, units, stock, stock places
- Projects, project types / states, timesheets, client services, communication kinds, notes, tasks
- Accounts, account groups, countries, currencies, languages, payment types, users, fictional users, company profile

#### API 3.0 (`bexio_lib.Interfaces.V3`)
- Currencies (+ exchange rates), taxes, users (+ `me`)
- Accounting: calendar years, business years, VAT periods, manual entries (+ next reference number), journal
- Banking: bank accounts
- Files: list, upload, download, delete

#### API 4.0 (`bexio_lib.Interfaces.V4`)
- Purchase bills, expenses, outgoing payments
- Payroll employees and absences

(uuid string ids, edit via PUT)

Entities with write access (`IBexioApiCrudEndpoint`) support `Create`, `Update` and `Delete`; all others `GetById`, `GetAll` and `Search`.
Resources not covered yet can be added by deriving from `BexioApiCrudEndpoint<T>` / `BexioApiFullEndpoint<T>` with the matching `BexioApiVersion`.

If you find something missing or broken, please [report an issue][github-issue] or even better fork the repo and submit a pull request


### How to use

#### Setup
Inject the services in your Startup.cs.
For now only the JWT authentication is supported. 

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBexioJwt(this.Configuration);
    ...
}
```

or manually setup the services

```csharp
var bexioApi = BexioApi.UseJwt("https://api.bexio.com", "myApiKey");
var bexioOrder = new BexioApiOrderEndpoint(bexioApi);
...
```

The setup method "AddBexioJwt" requires you to provide bexioApiKey (and optionally bexioApiUrl, default https://api.bexio.com; a trailing version like `/2.0` is ignored) through either environment variables or a configuration file.

```json 
A snipped from launchSettings.json

{
   "YourProject": {
      "environmentVariables": {
        "bexioApiUrl": "https://api.bexio.com",
        "bexioApiKey": "....."
      }
   }
}
```

#### Consume api

Inject the needed endpoint services in your constructor and you are ready to go

```csharp
[Route("api/[controller]")]
[ApiController]
public class LeadsController : ControllerBase
{
    private readonly IBexioApiInvoiceEndpoint _bexioInvoices;

    public LeadsController(IBexioApiInvoiceEndpoint bexioInvoiceEndpoint)
    {
        this._bexioInvoices = bexioInvoiceEndpoint;
    }

    [HttpPost]
    public IActionResult FilterLeads([FromBody] QueryLeadsViewModel vm)
    {

        var invoices = this._bexioInvoices.Search(new BexioRequestFilter()
            .Add(new BexioRequestFilterInstruction(
                BexioInvoiceFilterFields.kb_item_status_id,
                $"{BexioInvoiceStatus.PAID},{BexioInvoiceStatus.PENDING},{BexioInvoiceStatus.PARTIAL}",
                BexioFilterCriteria.IN
                ))
            .Add(new BexioRequestFilterInstruction(
                BexioInvoiceFilterFields.is_valid_from,
                $"{vm.FromDate.ToString("o", CultureInfo.InvariantCulture)}",
                BexioFilterCriteria.GREATER_THAN
                ))
            .Add(new BexioRequestFilterInstruction(
                BexioInvoiceFilterFields.is_valid_to,
                $"{vm.ToDate.ToString("o", CultureInfo.InvariantCulture)}",
                BexioFilterCriteria.LESS_THAN
                ))
            );
        return Ok(invoices);
    }
}
```


#### Async, v3 and CRUD

```csharp
var currencies = await _currencyV3.GetAllAsync();                  // api 3.0
var contact = await _contacts.CreateAsync(new BexioContact { name_1 = "Muster AG", contact_type_id = BexioContactTypes.COMPANY, user_id = 1, owner_id = 1 });
```

Failed requests throw a `BexioApiException` with `StatusCode` and the raw `Content`.

### Tests
`dotnet test bexio-api/BexioLibTest` runs unit tests against a fake api. Integration tests run only if `bexioApiKey` is set as environment variable.

### Documentation
Coming soon
