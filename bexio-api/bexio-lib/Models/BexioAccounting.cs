using System.Collections.Generic;

namespace bexio_lib.Models
{
    public class BexioAccount
    {
        public int? id { get; set; }
        public string account_no { get; set; }
        public string name { get; set; }
        public int? account_group_id { get; set; }
        public int? account_type { get; set; }
        public int? tax_id { get; set; }
        public bool? is_active { get; set; }
        public bool? is_locked { get; set; }
    }

    public class BexioAccountGroup
    {
        public int? id { get; set; }
        public string account_no { get; set; }
        public string name { get; set; }
        public int? parent_fibu_account_group_id { get; set; }
        public bool? is_active { get; set; }
        public bool? is_locked { get; set; }
    }

    public class BexioCalendarYear
    {
        public int? id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool? is_vat_subject { get; set; }
        public int? total_reserved_amount { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int? vat_accounting_method { get; set; }
        public int? vat_accounting_type { get; set; }
    }

    public class BexioBusinessYear
    {
        public int? id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class BexioVatPeriod
    {
        public int? id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string closed_at { get; set; }
    }

    public class BexioTax
    {
        public int? id { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string digit { get; set; }
        public float? type { get; set; }
        public float? account_id { get; set; }
        public string tax_settlement_type { get; set; }
        public float? value { get; set; }
        public float? net_tax_value { get; set; }
        public string start_year { get; set; }
        public string end_year { get; set; }
        public bool? is_active { get; set; }
        public string display_name { get; set; }
    }

    public class BexioManualEntry
    {
        public int? id { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string reference_nr { get; set; }
        public string info { get; set; }
        public bool? is_locked { get; set; }
        public string locked_info { get; set; }
        public string created_at { get; set; }
        public ICollection<BexioManualEntryLine> entries { get; set; }
    }

    public class BexioManualEntryLine
    {
        public int? id { get; set; }
        public int? debit_account_id { get; set; }
        public int? credit_account_id { get; set; }
        public int? tax_id { get; set; }
        public int? tax_account_id { get; set; }
        public string description { get; set; }
        public float? amount { get; set; }
        public int? currency_id { get; set; }
        public float? currency_factor { get; set; }
    }

    public class BexioNextReferenceNumber
    {
        public string next_ref_nr { get; set; }
    }

    public class BexioJournalEntry
    {
        public int? id { get; set; }
        public string date { get; set; }
        public string ref_no { get; set; }
        public string booking_text { get; set; }
        public int? debit_account_id { get; set; }
        public int? credit_account_id { get; set; }
        public float? amount { get; set; }
        public int? currency_id { get; set; }
        public float? currency_factor { get; set; }
        public int? tax_id { get; set; }
    }

    public class BexioBankAccount
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string owner_address { get; set; }
        public string owner_zip { get; set; }
        public string owner_city { get; set; }
        public string owner_country_code { get; set; }
        public string iban { get; set; }
        public string bic_swift { get; set; }
        public string currency_code { get; set; }
        public int? currency_id { get; set; }
        public string account_id { get; set; }
    }

    public class BexioFile
    {
        public int? id { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public int? size_in_bytes { get; set; }
        public string extension { get; set; }
        public string mime_type { get; set; }
        public string uploader_email { get; set; }
        public int? user_id { get; set; }
        public bool? is_archived { get; set; }
        public int? source_id { get; set; }
        public string source_type { get; set; }
        public bool? is_referenced { get; set; }
        public string created_at { get; set; }
    }

    public class BexioExchangeRate
    {
        public string currency_code { get; set; }
        public string date { get; set; }
        public float? factor { get; set; }
    }
}
