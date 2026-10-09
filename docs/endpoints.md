# Endpoint reference

All endpoints are reachable through `IBexioClient` (`client.V2.Contacts`, ...) and can also be injected one by one via their interface.
Every endpoint has sync and `...Async` methods.

Access levels:
- **read**: `GetById`, `GetAll`, `Search` (+ async variants)
- **CRUD**: read plus `Create`, `Update`, `Delete`. api 2.0 edits with `POST`, 3.0 and 4.0 with `PUT`.
- **CRUD (string ids)**: api 4.0 resources use uuid string ids: `GetById(string)`, `Update(string, ...)`, `Delete(string)`.
- Other entries list the extra actions of the endpoint.

> The api 3.0 and 4.0 endpoints and the models of the newer resources were written without access to the bexio documentation and have not been verified against the real API yet. Please open an issue if a path or field does not match.

## API 2.0

| `client.V2.…` | Interface | Resource | Access |
|---|---|---|---|
| `Accounts` | `IBexioApiAccountEndpoint` | `2.0/accounts` | read |
| `AccountGroups` | `IBexioApiAccountGroupEndpoint` | `2.0/account_groups` | read |
| `AdditionalAddresses` | `IBexioApiAdditionalAddressEndpoint` | `2.0/contact` | nested CRUD per contact |
| `Articles` | `IBexioApiArticleEndpoint` | `2.0/article` | CRUD |
| `ArticleTypes` | `IBexioApiArticleTypeEndpoint` | `2.0/article_type` | read |
| `ClientServices` | `IBexioApiClientServiceEndpoint` | `2.0/client_service` | CRUD |
| `CommunicationKinds` | `IBexioApiCommunicationKindEndpoint` | `2.0/communication_kind` | read |
| `CompanyProfile` | `IBexioApiCompanyProfileEndpoint` | `2.0/company_profile` | read |
| `Contacts` | `IBexioApiContactEndpoint` | `2.0/contact` | CRUD |
| `ContactGroups` | `IBexioApiContactGroupEndpoint` | `2.0/contact_group` | CRUD |
| `ContactRelations` | `IBexioApiContactRelationEndpoint` | `2.0/contact_relation` | CRUD |
| `ContactSectors` | `IBexioApiContactSectorEndpoint` | `2.0/contact_branch` | read |
| `Countries` | `IBexioApiCountryEndpoint` | `2.0/country` | CRUD |
| `Currencies` | `IBexioApiCurrencyEndpoint` | `2.0/currency` | CRUD + exchange rates |
| `Deliveries` | `IBexioApiDeliveryEndpoint` | `2.0/kb_delivery` | read + issue |
| `FictionalUsers` | `IBexioApiFictionalUserEndpoint` | `2.0/fictional_user` | CRUD |
| `Invoices` | `IBexioApiInvoiceEndpoint` | `2.0/kb_invoice` | CRUD + actions |
| `InvoicePayments` | `IBexioApiInvoicePaymentEndpoint` | `2.0/kb_invoice` | nested, per invoice |
| `InvoicePositions` | `IBexioApiInvoicePositionEndpoint` | `2.0/kb_invoice` | nested CRUD per document |
| `Languages` | `IBexioApiLanguageEndpoint` | `2.0/language` | CRUD |
| `Notes` | `IBexioApiNoteEndpoint` | `2.0/note` | CRUD |
| `Offers` | `IBexioApiOfferEndpoint` | `2.0/kb_offer` | CRUD + actions |
| `OfferPositions` | `IBexioApiOfferPositionEndpoint` | `2.0/kb_offer` | nested CRUD per document |
| `Orders` | `IBexioApiOrderEndpoint` | `2.0/kb_order` | CRUD + actions |
| `OrderPositions` | `IBexioApiOrderPositionEndpoint` | `2.0/kb_order` | nested CRUD per document |
| `PaymentTypes` | `IBexioApiPaymentTypeEndpoint` | `2.0/payment_type` | read |
| `Projects` | `IBexioApiProjectEndpoint` | `2.0/pr_project` | CRUD + archive |
| `ProjectStatuses` | `IBexioApiProjectStatusEndpoint` | `2.0/pr_project_state` | read |
| `ProjectTypes` | `IBexioApiProjectTypeEndpoint` | `2.0/pr_project_type` | read |
| `Salutations` | `IBexioApiSalutationEndpoint` | `2.0/salutation` | CRUD |
| `Stocks` | `IBexioApiStockEndpoint` | `2.0/stock` | read |
| `StockPlaces` | `IBexioApiStockPlaceEndpoint` | `2.0/stock_place` | read |
| `Tasks` | `IBexioApiTaskEndpoint` | `2.0/task` | CRUD |
| `Timesheets` | `IBexioApiTimesheetEndpoint` | `2.0/timesheet` | CRUD |
| `TimesheetStatuses` | `IBexioApiTimesheetStatusEndpoint` | `2.0/timesheet_status` | read |
| `Titles` | `IBexioApiTitleEndpoint` | `2.0/title` | CRUD |
| `Units` | `IBexioApiUnitEndpoint` | `2.0/unit` | CRUD |
| `Users` | `IBexioApiUserEndpoint` | `2.0/user` | read, `GetMe()` |

## API 3.0

| `client.V3.…` | Interface | Resource | Access |
|---|---|---|---|
| `BankAccounts` | `IBexioApiBankAccountEndpoint` | `3.0/banking/accounts` | read |
| `BusinessYears` | `IBexioApiBusinessYearEndpoint` | `3.0/accounting/business_years` | read |
| `CalendarYears` | `IBexioApiCalendarYearEndpoint` | `3.0/accounting/calendar_years` | read, create |
| `Currencies` | `IBexioApiCurrencyV3Endpoint` | `3.0/currencies` | CRUD + exchange rates |
| `Files` | `IBexioApiFileEndpoint` | `3.0/files` | read, upload, download, delete |
| `Journal` | `IBexioApiJournalEndpoint` | `3.0/accounting/journal` | list (`from` / `to`) |
| `ManualEntries` | `IBexioApiManualEntryEndpoint` | `3.0/accounting/manual_entries` | CRUD + next reference number |
| `Taxes` | `IBexioApiTaxEndpoint` | `3.0/taxes` | read, delete |
| `Users` | `IBexioApiUserV3Endpoint` | `3.0/users` | read, `GetMe()` |
| `VatPeriods` | `IBexioApiVatPeriodEndpoint` | `3.0/accounting/vat_periods` | read |

## API 4.0

| `client.V4.…` | Interface | Resource | Access |
|---|---|---|---|
| `Absences` | `IBexioApiAbsenceEndpoint` | `4.0/payroll/absences` | CRUD (string ids) |
| `Bills` | `IBexioApiBillEndpoint` | `4.0/purchase/bills` | CRUD (string ids) |
| `Employees` | `IBexioApiEmployeeEndpoint` | `4.0/payroll/employees` | CRUD (string ids) |
| `Expenses` | `IBexioApiExpenseEndpoint` | `4.0/expenses` | CRUD (string ids) |
| `OutgoingPayments` | `IBexioApiOutgoingPaymentEndpoint` | `4.0/purchase/outgoing-payments` | CRUD (string ids) |

Missing a resource? Derive from `BexioApiCrudEndpoint<T>` or `BexioApiFullEndpoint<T>`, pass the api version and path, and register it. See [Extending](usage.md#extending-with-your-own-endpoint).
