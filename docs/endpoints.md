# Endpoint reference

Generated from the official bexio OpenAPI description by `tools/generate.py`. Do not edit by hand.

Every operation of the bexio API 2.0 and 3.0 is available through `IBexioClient`: `client.V2.<Group>` and `client.V3.<Group>`.
Each operation exists as `Name(...)` and `NameAsync(..., CancellationToken)`. List and search operations with `limit` / `offset` also offer `NamePagesAsync(...)`, which reads all pages.
The method names are the operation ids of the bexio documentation. Groups with a main collection additionally offer the short forms
`GetAll`, `GetById`, `Search`, `Create`, `Update` and `Delete`.

Total: 272 operations in 50 groups.

## API 2.0

### `client.V2.AccountGroups`

Interface `IBexioApiAccountGroupsEndpoint`, 1 operations.

| Method | Request | Scope |
|---|---|---|
| `ListAccountGroups` / `GetAll` | `GET /2.0/account_groups` | - |

### `client.V2.Accounts`

Interface `IBexioApiAccountsEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListAccounts` / `GetAll` | `GET /2.0/accounts` | - |
| `SearchAccounts` / `Search` | `POST /2.0/accounts/search` | - |

### `client.V2.AdditionalAddresses`

Interface `IBexioApiAdditionalAddressesEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListAdditionalAddresses` | `GET /2.0/contact/{contact_id}/additional_address` | contact_show |
| `CreateAdditionalAddress` | `POST /2.0/contact/{contact_id}/additional_address` | contact_edit |
| `SearchAdditionalAddresses` | `POST /2.0/contact/{contact_id}/additional_address/search` | contact_show |
| `ShowAdditionalAddress` | `GET /2.0/contact/{contact_id}/additional_address/{additional_address_id}` | contact_show |
| `EditAdditionalAddress` | `POST /2.0/contact/{contact_id}/additional_address/{additional_address_id}` | contact_edit |
| `DeleteAdditionalAddress` | `DELETE /2.0/contact/{contact_id}/additional_address/{additional_address_id}` | contact_edit |

### `client.V2.BusinessActivities`

Interface `IBexioApiBusinessActivitiesEndpoint`, 3 operations.

| Method | Request | Scope |
|---|---|---|
| `ListBusinessActivities` / `GetAll` | `GET /2.0/client_service` | general |
| `CreateBusinessActivity` / `Create` | `POST /2.0/client_service` | general |
| `SearchBusinessActivities` / `Search` | `POST /2.0/client_service/search` | general |

### `client.V2.Comments`

Interface `IBexioApiCommentsEndpoint`, 3 operations.

| Method | Request | Scope |
|---|---|---|
| `ListComments` | `GET /2.0/{kb_document_type}/{document_id}/comment` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateComment` | `POST /2.0/{kb_document_type}/{document_id}/comment` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowComment` | `GET /2.0/{kb_document_type}/{document_id}/comment/{comment_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.CommunicationTypes`

Interface `IBexioApiCommunicationTypesEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListCommunicationTypes` / `GetAll` | `GET /2.0/communication_kind` | general |
| `SearchCommunicationTypes` / `Search` | `POST /2.0/communication_kind/search` | general |

### `client.V2.CompanyProfile`

Interface `IBexioApiCompanyProfileEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListCompanyProfile` | `GET /2.0/company_profile` | general |
| `ShowCompanyProfile` / `GetById` | `GET /2.0/company_profile/{profile_id}` | general |

### `client.V2.ContactGroups`

Interface `IBexioApiContactGroupsEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListContactGroups` / `GetAll` | `GET /2.0/contact_group` | general |
| `CreateContactGroup` / `Create` | `POST /2.0/contact_group` | general |
| `SearchContactGroups` / `Search` | `POST /2.0/contact_group/search` | general |
| `ShowContactGroup` / `GetById` | `GET /2.0/contact_group/{contact_group_id}` | general |
| `EditContactGroup` / `Update` | `POST /2.0/contact_group/{contact_group_id}` | general |
| `DeleteContactGroup` / `Delete` | `DELETE /2.0/contact_group/{contact_group_id}` | general |

### `client.V2.ContactRelations`

Interface `IBexioApiContactRelationsEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListContactRelations` / `GetAll` | `GET /2.0/contact_relation` | contact_show |
| `CreateContactRelation` / `Create` | `POST /2.0/contact_relation` | contact_edit |
| `SearchContactRelations` / `Search` | `POST /2.0/contact_relation/search` | contact_show |
| `ShowContactRelation` / `GetById` | `GET /2.0/contact_relation/{contact_relation_id}` | contact_show |
| `EditContactRelation` / `Update` | `POST /2.0/contact_relation/{contact_relation_id}` | contact_edit |
| `DeleteContactRelation` / `Delete` | `DELETE /2.0/contact_relation/{contact_relation_id}` | contact_edit |

### `client.V2.ContactSectors`

Interface `IBexioApiContactSectorsEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListContactSectors` / `GetAll` | `GET /2.0/contact_branch` | general |
| `SearchContactSectors` / `Search` | `POST /2.0/contact_branch/search` | general |

### `client.V2.Contacts`

Interface `IBexioApiContactsEndpoint`, 8 operations.

| Method | Request | Scope |
|---|---|---|
| `ListContacts` / `GetAll` | `GET /2.0/contact` | contact_show |
| `CreateContact` / `Create` | `POST /2.0/contact` | contact_edit |
| `BulkCreateContacts` | `POST /2.0/contact/_bulk_create` | contact_edit |
| `SearchContact` / `Search` | `POST /2.0/contact/search` | contact_show |
| `ShowContact` / `GetById` | `GET /2.0/contact/{contact_id}` | contact_show |
| `EditContact` / `Update` | `POST /2.0/contact/{contact_id}` | contact_edit |
| `DeleteContact` / `Delete` | `DELETE /2.0/contact/{contact_id}` | contact_edit |
| `RestoreContact` | `PATCH /2.0/contact/{contact_id}/restore` | contact_edit |

### `client.V2.Countries`

Interface `IBexioApiCountriesEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListCountries` / `GetAll` | `GET /2.0/country` | general |
| `CreateCountry` / `Create` | `POST /2.0/country` | general |
| `SearchCountries` / `Search` | `POST /2.0/country/search` | general |
| `ShowCountry` / `GetById` | `GET /2.0/country/{country_id}` | general |
| `EditCountry` / `Update` | `POST /2.0/country/{country_id}` | general |
| `DeleteCountry` / `Delete` | `DELETE /2.0/country/{country_id}` | general |

### `client.V2.DefaultPositions`

Interface `IBexioApiDefaultPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListDefaultPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_custom` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateDefaultPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_custom` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowDefaultPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_custom/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditDefaultPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_custom/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeleteDefaultPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_custom/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.Deliveries`

Interface `IBexioApiDeliveriesEndpoint`, 3 operations.

| Method | Request | Scope |
|---|---|---|
| `ListDeliveries` / `GetAll` | `GET /2.0/kb_delivery` | kb_delivery_show |
| `ShowDelivery` / `GetById` | `GET /2.0/kb_delivery/{delivery_id}` | kb_delivery_show |
| `IssueDelivery` | `POST /2.0/kb_delivery/{delivery_id}/issue` | kb_delivery_edit |

### `client.V2.DiscountPositions`

Interface `IBexioApiDiscountPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListDiscountPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_discount` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateDiscountPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_discount` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowDiscountPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_discount/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditDiscountPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_discount/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeleteDiscountPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_discount/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.DocumentSettings`

Interface `IBexioApiDocumentSettingsEndpoint`, 1 operations.

| Method | Request | Scope |
|---|---|---|
| `ListDocumentSettings` / `GetAll` | `GET /2.0/kb_item_setting` | general |

### `client.V2.Invoices`

Interface `IBexioApiInvoicesEndpoint`, 26 operations.

| Method | Request | Scope |
|---|---|---|
| `ListInvoices` / `GetAll` | `GET /2.0/kb_invoice` | kb_invoice_show |
| `CreateInvoice` / `Create` | `POST /2.0/kb_invoice` | kb_invoice_edit |
| `SearchInvoices` / `Search` | `POST /2.0/kb_invoice/search` | kb_invoice_show |
| `ShowInvoice` / `GetById` | `GET /2.0/kb_invoice/{invoice_id}` | kb_invoice_show |
| `EditInvoice` / `Update` | `POST /2.0/kb_invoice/{invoice_id}` | kb_invoice_edit |
| `DeleteInvoice` / `Delete` | `DELETE /2.0/kb_invoice/{invoice_id}` | kb_invoice_edit |
| `CancelInvoice` | `POST /2.0/kb_invoice/{invoice_id}/cancel` | kb_invoice_edit |
| `CopyInvoice` | `POST /2.0/kb_invoice/{invoice_id}/copy` | kb_invoice_edit |
| `IssueInvoice` | `POST /2.0/kb_invoice/{invoice_id}/issue` | kb_invoice_edit |
| `ListInvoiceReminders` | `GET /2.0/kb_invoice/{invoice_id}/kb_reminder` | kb_invoice_show |
| `CreateInvoiceReminder` | `POST /2.0/kb_invoice/{invoice_id}/kb_reminder` | kb_invoice_edit |
| `SearchReminders` | `POST /2.0/kb_invoice/{invoice_id}/kb_reminder/search` | kb_invoice_show |
| `ShowInvoiceReminder` | `GET /2.0/kb_invoice/{invoice_id}/kb_reminder/{reminder_id}` | kb_invoice_show |
| `DeleteInvoiceReminder` | `DELETE /2.0/kb_invoice/{invoice_id}/kb_reminder/{reminder_id}` | kb_invoice_edit |
| `MarkAsSentInvoiceReminder` | `POST /2.0/kb_invoice/{invoice_id}/kb_reminder/{reminder_id}/mark_as_sent` | kb_invoice_edit |
| `MarkAsUnsentInvoiceReminder` | `POST /2.0/kb_invoice/{invoice_id}/kb_reminder/{reminder_id}/mark_as_unsent` | kb_invoice_edit |
| `ShowInvoiceReminderPDF` | `GET /2.0/kb_invoice/{invoice_id}/kb_reminder/{reminder_id}/pdf` | kb_invoice_show |
| `SendInvoiceReminder` | `POST /2.0/kb_invoice/{invoice_id}/kb_reminder/{reminder_id}/send` | kb_invoice_edit |
| `MarkAsSentInvoice` | `POST /2.0/kb_invoice/{invoice_id}/mark_as_sent` | kb_invoice_edit |
| `ListInvoicePayments` | `GET /2.0/kb_invoice/{invoice_id}/payment` | kb_invoice_show |
| `CreateInvoicePayment` | `POST /2.0/kb_invoice/{invoice_id}/payment` | kb_invoice_edit |
| `ShowInvoicePayment` | `GET /2.0/kb_invoice/{invoice_id}/payment/{payment_id}` | kb_invoice_show |
| `DeleteInvoicePayment` | `DELETE /2.0/kb_invoice/{invoice_id}/payment/{payment_id}` | kb_invoice_edit |
| `ShowInvoicePDF` | `GET /2.0/kb_invoice/{invoice_id}/pdf` | kb_invoice_show |
| `RevertIssueInvoice` | `POST /2.0/kb_invoice/{invoice_id}/revert_issue` | kb_invoice_edit |
| `SendInvoice` | `POST /2.0/kb_invoice/{invoice_id}/send` | kb_invoice_edit |

### `client.V2.ItemPositions`

Interface `IBexioApiItemPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListItemPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_article` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateItemPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_article` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowItemPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_article/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditItemPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_article/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeleteItemPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_article/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.Items`

Interface `IBexioApiItemsEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListItems` / `GetAll` | `GET /2.0/article` | article_show |
| `CreateItem` / `Create` | `POST /2.0/article` | article_edit |
| `SearchItems` / `Search` | `POST /2.0/article/search` | article_show |
| `ShowItem` / `GetById` | `GET /2.0/article/{article_id}` | article_show |
| `EditItem` / `Update` | `POST /2.0/article/{article_id}` | article_edit |
| `DeleteItem` / `Delete` | `DELETE /2.0/article/{article_id}` | article_edit |

### `client.V2.Languages`

Interface `IBexioApiLanguagesEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListLanguages` / `GetAll` | `GET /2.0/language` | general |
| `SearchLanguages` / `Search` | `POST /2.0/language/search` | general |

### `client.V2.Notes`

Interface `IBexioApiNotesEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListNotes` / `GetAll` | `GET /2.0/note` | note_show |
| `CreateNote` / `Create` | `POST /2.0/note` | note_edit |
| `SearchNotes` / `Search` | `POST /2.0/note/search` | note_show |
| `ShowNote` / `GetById` | `GET /2.0/note/{note_id}` | note_show |
| `EditNote` / `Update` | `POST /2.0/note/{note_id}` | note_edit |
| `DeleteNote` / `Delete` | `DELETE /2.0/note/{note_id}` | note_edit |

### `client.V2.Orders`

Interface `IBexioApiOrdersEndpoint`, 12 operations.

| Method | Request | Scope |
|---|---|---|
| `ListOrders` / `GetAll` | `GET /2.0/kb_order` | kb_order_show |
| `CreateOrder` / `Create` | `POST /2.0/kb_order` | kb_order_edit |
| `SearchOrders` / `Search` | `POST /2.0/kb_order/search` | kb_order_show |
| `ShowOrder` / `GetById` | `GET /2.0/kb_order/{order_id}` | kb_order_show |
| `EditOrder` / `Update` | `POST /2.0/kb_order/{order_id}` | kb_order_edit |
| `DeleteOrder` / `Delete` | `DELETE /2.0/kb_order/{order_id}` | kb_order_edit |
| `CreateDeliveryFromOrder` | `POST /2.0/kb_order/{order_id}/delivery` | kb_delivery_edit, kb_order_edit |
| `CreateInvoiceFromOrder` | `POST /2.0/kb_order/{order_id}/invoice` | kb_invoice_edit, kb_order_edit |
| `ShowOrderPDF` | `GET /2.0/kb_order/{order_id}/pdf` | kb_order_show |
| `ShowOrderRepetition` | `GET /2.0/kb_order/{order_id}/repetition` | kb_order_show |
| `EditOrderRepetition` | `POST /2.0/kb_order/{order_id}/repetition` | kb_order_edit |
| `DeleteOrderRepetition` | `DELETE /2.0/kb_order/{order_id}/repetition` | kb_order_edit |

### `client.V2.PagebreakPositions`

Interface `IBexioApiPagebreakPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListPagebreakPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_pagebreak` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreatePagebreakPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_pagebreak` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowPagebreakPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_pagebreak/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditPagebreakPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_pagebreak/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeletePagebreakPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_pagebreak/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.PaymentTypes`

Interface `IBexioApiPaymentTypesEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListPaymentTypes` / `GetAll` | `GET /2.0/payment_type` | general |
| `SearchPaymentTypes` / `Search` | `POST /2.0/payment_type/search` | general |

### `client.V2.Projects`

Interface `IBexioApiProjectsEndpoint`, 10 operations.

| Method | Request | Scope |
|---|---|---|
| `ListProjects` / `GetAll` | `GET /2.0/pr_project` | project_show |
| `CreateProject` / `Create` | `POST /2.0/pr_project` | project_edit |
| `SearchProjects` / `Search` | `POST /2.0/pr_project/search` | project_show |
| `ShowProject` / `GetById` | `GET /2.0/pr_project/{project_id}` | project_show |
| `EditProject` / `Update` | `POST /2.0/pr_project/{project_id}` | project_edit |
| `DeleteProject` / `Delete` | `DELETE /2.0/pr_project/{project_id}` | project_edit |
| `ArchiveProject` | `POST /2.0/pr_project/{project_id}/archive` | project_edit |
| `UnarchiveProject` | `POST /2.0/pr_project/{project_id}/reactivate` | project_edit |
| `ListProjectStatus` | `GET /2.0/pr_project_state` | general |
| `ListProjectType` | `GET /2.0/pr_project_type` | general |

### `client.V2.Quotes`

Interface `IBexioApiQuotesEndpoint`, 17 operations.

| Method | Request | Scope |
|---|---|---|
| `ListQuotes` / `GetAll` | `GET /2.0/kb_offer` | kb_offer_show |
| `CreateQuote` / `Create` | `POST /2.0/kb_offer` | kb_offer_edit |
| `SearchQuotes` / `Search` | `POST /2.0/kb_offer/search` | kb_offer_show |
| `ShowQuote` / `GetById` | `GET /2.0/kb_offer/{quote_id}` | kb_offer_show |
| `EditQuote` / `Update` | `POST /2.0/kb_offer/{quote_id}` | kb_offer_edit |
| `DeleteQuote` / `Delete` | `DELETE /2.0/kb_offer/{quote_id}` | kb_offer_edit |
| `AcceptQuote` | `POST /2.0/kb_offer/{quote_id}/accept` | kb_offer_edit |
| `CopyQuote` | `POST /2.0/kb_offer/{quote_id}/copy` | kb_offer_edit |
| `CreateInvoiceFromQuote` | `POST /2.0/kb_offer/{quote_id}/invoice` | kb_invoice_edit, kb_offer_edit |
| `IssueQuote` | `POST /2.0/kb_offer/{quote_id}/issue` | kb_offer_edit |
| `MarkAsSentQuote` | `POST /2.0/kb_offer/{quote_id}/mark_as_sent` | kb_offer_edit |
| `CreateOrderFromQuote` | `POST /2.0/kb_offer/{quote_id}/order` | kb_offer_edit, kb_order_edit |
| `ShowQuotePDF` | `GET /2.0/kb_offer/{quote_id}/pdf` | kb_offer_show |
| `ReissueQuote` | `POST /2.0/kb_offer/{quote_id}/reissue` | kb_offer_edit |
| `DeclineQuote` | `POST /2.0/kb_offer/{quote_id}/reject` | kb_offer_edit |
| `RevertIssueQuote` | `POST /2.0/kb_offer/{quote_id}/revertIssue` | kb_offer_edit |
| `SendQuote` | `POST /2.0/kb_offer/{quote_id}/send` | kb_offer_edit |

### `client.V2.Salutations`

Interface `IBexioApiSalutationsEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListSalutations` / `GetAll` | `GET /2.0/salutation` | general |
| `CreateSalutation` / `Create` | `POST /2.0/salutation` | general |
| `SearchSalutations` / `Search` | `POST /2.0/salutation/search` | general |
| `ShowSalutation` / `GetById` | `GET /2.0/salutation/{salutation_id}` | general |
| `EditSalutation` / `Update` | `POST /2.0/salutation/{salutation_id}` | general |
| `DeleteSalutation` / `Delete` | `DELETE /2.0/salutation/{salutation_id}` | general |

### `client.V2.StockAreas`

Interface `IBexioApiStockAreasEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListStockAreas` / `GetAll` | `GET /2.0/stock_place` | stock_edit |
| `SearchStockAreas` / `Search` | `POST /2.0/stock_place/search` | stock_edit |

### `client.V2.StockLocations`

Interface `IBexioApiStockLocationsEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListStockLocations` / `GetAll` | `GET /2.0/stock` | stock_edit |
| `SearchStockLocations` / `Search` | `POST /2.0/stock/search` | stock_edit |

### `client.V2.SubPositions`

Interface `IBexioApiSubPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListSubpositionPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_subposition` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateSubpositionPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_subposition` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowSubpositionPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_subposition/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditSubpositionPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_subposition/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeleteSubpositionPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_subposition/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.SubtotalPositions`

Interface `IBexioApiSubtotalPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListSubtotalPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_subtotal` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateSubtotalPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_subtotal` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowSubtotalPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_subtotal/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditSubtotalPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_subtotal/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeleteSubtotalPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_subtotal/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.Tasks`

Interface `IBexioApiTasksEndpoint`, 8 operations.

| Method | Request | Scope |
|---|---|---|
| `ListTasks` / `GetAll` | `GET /2.0/task` | task_show |
| `CreateTask` / `Create` | `POST /2.0/task` | task_edit |
| `SearchTasks` / `Search` | `POST /2.0/task/search` | task_show |
| `ShowTask` / `GetById` | `GET /2.0/task/{task_id}` | task_show |
| `EditTask` / `Update` | `POST /2.0/task/{task_id}` | task_show |
| `DeleteTask` / `Delete` | `DELETE /2.0/task/{task_id}` | task_edit |
| `ListTaskPriorities` | `GET /2.0/todo_priority` | task_show |
| `ListTaskStatus` | `GET /2.0/todo_status` | task_show |

### `client.V2.TextPositions`

Interface `IBexioApiTextPositionsEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `ListTextPositions` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_text` | kb_invoice_show, kb_offer_show, kb_order_show |
| `CreateTextPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_text` | kb_invoice_edit, kb_offer_edit, kb_order_edit |
| `ShowTextPosition` | `GET /2.0/{kb_document_type}/{document_id}/kb_position_text/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `EditTextPosition` | `POST /2.0/{kb_document_type}/{document_id}/kb_position_text/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |
| `DeleteTextPosition` | `DELETE /2.0/{kb_document_type}/{document_id}/kb_position_text/{position_id}` | kb_invoice_show, kb_offer_show, kb_order_show |

### `client.V2.Timesheets`

Interface `IBexioApiTimesheetsEndpoint`, 7 operations.

| Method | Request | Scope |
|---|---|---|
| `ListTimesheets` / `GetAll` | `GET /2.0/timesheet` | monitoring_show |
| `CreateTimesheet` / `Create` | `POST /2.0/timesheet` | monitoring_edit |
| `SearchTimesheets` / `Search` | `POST /2.0/timesheet/search` | monitoring_show |
| `ShowTimesheet` / `GetById` | `GET /2.0/timesheet/{timesheet_id}` | monitoring_show |
| `EditTimesheet` / `Update` | `POST /2.0/timesheet/{timesheet_id}` | monitoring_edit |
| `DeleteTimesheet` / `Delete` | `DELETE /2.0/timesheet/{timesheet_id}` | monitoring_edit |
| `ListTimeSheetStatus` | `GET /2.0/timesheet_status` | general |

### `client.V2.Titles`

Interface `IBexioApiTitlesEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListTitles` / `GetAll` | `GET /2.0/title` | general |
| `CreateTitle` / `Create` | `POST /2.0/title` | general |
| `SearchTitles` / `Search` | `POST /2.0/title/search` | general |
| `ShowTitle` / `GetById` | `GET /2.0/title/{title_id}` | general |
| `EditTitle` / `Update` | `POST /2.0/title/{title_id}` | general |
| `DeleteTitle` / `Delete` | `DELETE /2.0/title/{title_id}` | general |

### `client.V2.Units`

Interface `IBexioApiUnitsEndpoint`, 6 operations.

| Method | Request | Scope |
|---|---|---|
| `ListUnits` / `GetAll` | `GET /2.0/unit` | general |
| `CreateUnit` / `Create` | `POST /2.0/unit` | general |
| `SearchUnits` / `Search` | `POST /2.0/unit/search` | general |
| `ShowUnit` / `GetById` | `GET /2.0/unit/{unit_id}` | general |
| `EditUnit` / `Update` | `POST /2.0/unit/{unit_id}` | general |
| `DeleteUnit` / `Delete` | `DELETE /2.0/unit/{unit_id}` | general |

## API 3.0

### `client.V3.BankAccounts`

Interface `IBexioApiBankAccountsEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListBankAccounts` / `GetAll` | `GET /3.0/banking/accounts` | bank_account_show |
| `ShowBankAccount` / `GetById` | `GET /3.0/banking/accounts/{bank_account_id}` | bank_account_show |

### `client.V3.BusinessYears`

Interface `IBexioApiBusinessYearsEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListBusinessYears` / `GetAll` | `GET /3.0/accounting/business_years` | - |
| `ShowBusinessYear` / `GetById` | `GET /3.0/accounting/business_years/{business_year_id}` | - |

### `client.V3.CalendarYears`

Interface `IBexioApiCalendarYearsEndpoint`, 4 operations.

| Method | Request | Scope |
|---|---|---|
| `ListCalendarYears` / `GetAll` | `GET /3.0/accounting/calendar_years` | - |
| `CreateCalendarYear` / `Create` | `POST /3.0/accounting/calendar_years` | - |
| `SearchCalendarYears` / `Search` | `POST /3.0/accounting/calendar_years/search` | - |
| `ShowCalendarYear` / `GetById` | `GET /3.0/accounting/calendar_years/{calendar_year_id}` | - |

### `client.V3.Currencies`

Interface `IBexioApiCurrenciesEndpoint`, 7 operations.

| Method | Request | Scope |
|---|---|---|
| `ListCurrencies` / `GetAll` | `GET /3.0/currencies` | - |
| `CreateCurrency` / `Create` | `POST /3.0/currencies` | - |
| `ListCurrenciesCodes` | `GET /3.0/currencies/codes` | - |
| `ShowCurrency` / `GetById` | `GET /3.0/currencies/{currency_id}` | - |
| `UpdateCurrency` / `Update` | `PATCH /3.0/currencies/{currency_id}` | - |
| `DeleteCurrency` / `Delete` | `DELETE /3.0/currencies/{currency_id}` | - |
| `ListExchangeRatesForCurrency` | `GET /3.0/currencies/{currency_id}/exchange_rates` | - |

### `client.V3.DocumentTemplates`

Interface `IBexioApiDocumentTemplatesEndpoint`, 1 operations.

| Method | Request | Scope |
|---|---|---|
| `ListDocumentTemplate` | `GET /3.0/document_templates` | - |

### `client.V3.Files`

Interface `IBexioApiFilesEndpoint`, 9 operations.

| Method | Request | Scope |
|---|---|---|
| `ReadFiles` / `GetAll` | `GET /3.0/files` | file |
| `CreateFile` | `POST /3.0/files` | file |
| `SearchFile` / `Search` | `POST /3.0/files/search` | file |
| `ReadFile` / `GetById` | `GET /3.0/files/{file_id}` | file |
| `UpdateFile` / `Update` | `PATCH /3.0/files/{file_id}` | file |
| `DeleteFile` / `Delete` | `DELETE /3.0/files/{file_id}` | file |
| `DownloadFile` | `GET /3.0/files/{file_id}/download` | file |
| `PreviewFile` | `GET /3.0/files/{file_id}/preview` | file |
| `ShowFile` | `GET /3.0/files/{file_id}/usage` | file |

### `client.V3.ManualEntries`

Interface `IBexioApiManualEntriesEndpoint`, 13 operations.

| Method | Request | Scope |
|---|---|---|
| `ListManualEntries` / `GetAll` | `GET /3.0/accounting/manual_entries` | accounting |
| `CreateManualEntry` / `Create` | `POST /3.0/accounting/manual_entries` | accounting |
| `GetNextReferenceNumber` | `GET /3.0/accounting/manual_entries/next_ref_nr` | accounting |
| `UpdateManualEntry` / `Update` | `PUT /3.0/accounting/manual_entries/{manual_entry_id}` | accounting |
| `DeleteManualEntry` / `Delete` | `DELETE /3.0/accounting/manual_entries/{manual_entry_id}` | - |
| `ListManualEntryFiles` | `GET /3.0/accounting/manual_entries/{manual_entry_id}/entries/{entry_id}/files` | - |
| `UploadManualEntryFile` | `POST /3.0/accounting/manual_entries/{manual_entry_id}/entries/{entry_id}/files` | - |
| `ShowManualEntryFile` | `GET /3.0/accounting/manual_entries/{manual_entry_id}/entries/{entry_id}/files/{file_id}` | - |
| `DeleteManualEntryFile` | `DELETE /3.0/accounting/manual_entries/{manual_entry_id}/entries/{entry_id}/files/{file_id}` | - |
| `ListManualCompoundEntryFiles` | `GET /3.0/accounting/manual_entries/{manual_entry_id}/files` | - |
| `UploadManualCompoundEntryFile` | `POST /3.0/accounting/manual_entries/{manual_entry_id}/files` | - |
| `ShowManualCompoundEntryFile` | `GET /3.0/accounting/manual_entries/{manual_entry_id}/files/{file_id}` | - |
| `DeleteManualCompoundEntryFile` | `DELETE /3.0/accounting/manual_entries/{manual_entry_id}/files/{file_id}` | - |

### `client.V3.Permissions`

Interface `IBexioApiPermissionsEndpoint`, 1 operations.

| Method | Request | Scope |
|---|---|---|
| `Permissions` | `GET /3.0/permissions` | general |

### `client.V3.Projects`

Interface `IBexioApiProjectsEndpoint`, 10 operations.

| Method | Request | Scope |
|---|---|---|
| `ListMilestones` | `GET /3.0/projects/{project_id}/milestones` | project_show |
| `CreateMilestone` | `POST /3.0/projects/{project_id}/milestones` | project_edit |
| `ShowMilestone` | `GET /3.0/projects/{project_id}/milestones/{milestone_id}` | project_show |
| `EditMilestone` | `POST /3.0/projects/{project_id}/milestones/{milestone_id}` | project_edit |
| `DeleteMilestone` | `DELETE /3.0/projects/{project_id}/milestones/{milestone_id}` | project_edit |
| `ListWorkPackages` | `GET /3.0/projects/{project_id}/packages` | project_show |
| `CreateWorkPackage` | `POST /3.0/projects/{project_id}/packages` | project_edit |
| `ShowWorkPackage` | `GET /3.0/projects/{project_id}/packages/{package_id}` | project_show |
| `EditWorkPackage` | `PATCH /3.0/projects/{project_id}/packages/{package_id}` | project_edit |
| `DeleteWorkPackage` | `DELETE /3.0/projects/{project_id}/packages/{package_id}` | project_edit |

### `client.V3.PurchaseOrders`

Interface `IBexioApiPurchaseOrdersEndpoint`, 5 operations.

| Method | Request | Scope |
|---|---|---|
| `PurchaseOrderList` / `GetAll` | `GET /3.0/purchase_orders` | kb_article_order_show |
| `PurchaseOrderCreate` / `Create` | `POST /3.0/purchase_orders` | kb_article_order_edit |
| `PurchaseOrderShow` / `GetById` | `GET /3.0/purchase_orders/{purchase_order_id}` | kb_article_order_show |
| `PurchaseOrderUpdate` / `Update` | `PUT /3.0/purchase_orders/{purchase_order_id}` | kb_article_order_edit |
| `PurchaseOrderDelete` / `Delete` | `DELETE /3.0/purchase_orders/{purchase_order_id}` | kb_article_order_edit |

### `client.V3.Reports`

Interface `IBexioApiReportsEndpoint`, 1 operations.

| Method | Request | Scope |
|---|---|---|
| `ListJournalEntries` / `GetAll` | `GET /3.0/accounting/journal` | accounting |

### `client.V3.Taxes`

Interface `IBexioApiTaxesEndpoint`, 3 operations.

| Method | Request | Scope |
|---|---|---|
| `ListTaxes` / `GetAll` | `GET /3.0/taxes` | - |
| `ShowTax` / `GetById` | `GET /3.0/taxes/{tax_id}` | - |
| `DeleteTax` / `Delete` | `DELETE /3.0/taxes/{tax_id}` | - |

### `client.V3.UserManagement`

Interface `IBexioApiUserManagementEndpoint`, 8 operations.

| Method | Request | Scope |
|---|---|---|
| `ListFictionalUsers` / `GetAll` | `GET /3.0/fictional_users` | general |
| `CreateFictionalUser` / `Create` | `POST /3.0/fictional_users` | general |
| `ShowFictionalUser` / `GetById` | `GET /3.0/fictional_users/{fictional_user_id}` | general |
| `UpdateFictionalUser` / `Update` | `PATCH /3.0/fictional_users/{fictional_user_id}` | general |
| `DeleteFictionalUser` / `Delete` | `DELETE /3.0/fictional_users/{fictional_user_id}` | general |
| `ListUsers` | `GET /3.0/users` | general |
| `ShowMe` | `GET /3.0/users/me` | general |
| `ShowUser` | `GET /3.0/users/{user_id}` | general |

### `client.V3.VatPeriods`

Interface `IBexioApiVatPeriodsEndpoint`, 2 operations.

| Method | Request | Scope |
|---|---|---|
| `ListVatPeriods` / `GetAll` | `GET /3.0/accounting/vat_periods` | - |
| `ShowVatPeriod` / `GetById` | `GET /3.0/accounting/vat_periods/{vat_period_id}` | - |

