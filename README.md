
# dotnet core Bexio client

This repository contains a dotnet / dotnet core compatible Bexio client.

Currently working is:
- GetById, GetAll, Search for Orders, Invoices and Deliveries
- Get invoice PDF
- Send invoice mail
- Issue invoice
- Mark invoice as sent
- Issue delivery

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
var bexioApi = BexioApi.UseJwt("url", "myApiKey");
var bexioOrder = new BexioApiOrderEndpoint(bexioApi);
...
```

The setup method "AddBexioJwt" requires you to provide bexioApiKey and bexioApiUrl through either environment variables or a configuration file.

```json 
A snipped from launchSettings.json

{
   "YourProject": {
      "environmentVariables": {
        "bexioApiUrl": "https://api.bexio.com/2.0",
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


### Documentation
Coming soon
