# Development

## Generated code

Models, endpoints, the `IBexioClient` facade and the search field constants are **generated** from the official OpenAPI description of the bexio API:

```
python3 tools/generate.py
```

| Input | Output |
|---|---|
| `docs/openapi/bexio-openapi.json` | `bexio-api/bexio-lib/Generated/**` (models, one file per endpoint group, facade, `BexioSearchFields`) |
| | `docs/openapi/operations.json` (manifest used by the tests) |
| | `docs/endpoints.md` (endpoint reference) |
| | `bexio-api/BexioLibTest/Generated/model-examples.json` (examples used by the tests) |

Never edit files in `Generated/`; change `tools/generate.py` instead. CI runs the script and fails when the committed output differs, so generated code and specification cannot drift apart.

### What the generator does

- one endpoint class per tag and api version, one method per operation (`Name` and `NameAsync`), named after the operation id of the documentation
- models from the (inlined) schemas: identical structures share one class, `allOf` is merged, `oneOf` / `anyOf` become `JToken`, all properties are nullable and named like the json
- list and search operations with `limit` / `offset` also get `...PagesAsync`
- short forms `GetAll`, `GetById`, `Search`, `Create`, `Update`, `Delete` for the main collection of a group
- the documentation is not always consistent. The generator reports and works around the cases it knows (a path parameter that is not declared, a number whose example is a text) as warnings; see the output of the script

### Updating to a new version of the API

1. Replace `docs/openapi/bexio-openapi.json` with the new description. Only `/2.0/` and `/3.0/` paths are used; the file holds the paths with operations and no `x-codeSamples`.
2. Run `python3 tools/generate.py`, `dotnet build` and `dotnet test`.
3. Review the diff. The test `Every_operation_of_the_spec_is_implemented` and the data driven tests tell you if an operation or a model does not match.

The specification is the one embedded in https://docs.bexio.com/ (version label 3.0.0 / 3.0.2). The copy in this repository was taken from a snapshot of that page from 2026-10-02.
