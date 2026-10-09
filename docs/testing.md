# Testing

## Your own code

Endpoints are interfaces, mock them with any library:

```csharp
var contacts = Substitute.For<IBexioApiContactsEndpoint>();
contacts.GetByIdAsync(1).Returns(new BexioContactWithDetails { id = 1, name_1 = "Muster" });
```

Or fake the transport and keep the real endpoint classes: implement `IBexioApi` (two methods: `Send` and `SendAsync`, they receive a `BexioRequest` with `Resource`, `Query`, `JsonBody`) and return a `BexioResponse`:

```csharp
public class FakeBexioApi : IBexioApi
{
    public string API_URL => "https://api.bexio.com";
    public BexioResponse Send(BexioRequest request, HttpMethod method) =>
        new() { StatusCode = HttpStatusCode.OK, Content = "[]" };
    public Task<BexioResponse> SendAsync(BexioRequest request, HttpMethod method, CancellationToken ct = default) =>
        Task.FromResult(this.Send(request, method));
}

var client = new BexioClient(new FakeBexioApi());
```

For tests of the HTTP layer pass an `HttpClient` with your own `HttpMessageHandler` to `new BexioApi(httpClient, options)`.

## The library's own tests

```
dotnet test bexio-api/bexio-api.sln
```

- `BexioLibTest/Unit`: every one of the 272 operations against the official OpenAPI description (method, path, async variant), every model against the examples of the description, endpoints against a fake transport, transport (url, headers, retry, multipart, paging) against a stubbed `HttpMessageHandler`, OAuth flow, DI wiring.
- `BexioLibTest/Integration`: run against the real api, only if the environment variable `bexioApiKey` is set, otherwise they pass without calling anything.
