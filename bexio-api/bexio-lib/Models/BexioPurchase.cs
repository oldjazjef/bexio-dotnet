using System.Collections.Generic;

namespace bexio_lib.Models
{
    /// <summary>Supplier bill (api 4.0 purchase/bills)</summary>
    public class BexioBill
    {
        public string id { get; set; }
        public int? supplier_id { get; set; }
        public int? contact_partner_id { get; set; }
        public string document_no { get; set; }
        public string title { get; set; }
        public string vendor_ref { get; set; }
        public string bill_date { get; set; }
        public string due_date { get; set; }
        public string currency_code { get; set; }
        public float? exchange_rate { get; set; }
        public float? manual_amount { get; set; }
        public float? amount_man { get; set; }
        public float? amount_calc { get; set; }
        public float? item_net { get; set; }
        public bool? vat_man { get; set; }
        public float? vat_calc { get; set; }
        public string status { get; set; }
        public float? pending_amount { get; set; }
        public ICollection<string> attachment_ids { get; set; }
        public BexioBillAddress address { get; set; }
        public ICollection<BexioBillLineItem> line_items { get; set; }
        public BexioBillPayment payment { get; set; }
    }

    public class BexioBillAddress
    {
        public string lastname_company { get; set; }
        public string firstname_suffix { get; set; }
        public string address_line { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
    }

    public class BexioBillLineItem
    {
        public int? id { get; set; }
        public int? position { get; set; }
        public string title { get; set; }
        public float? amount { get; set; }
        public int? tax_id { get; set; }
        public int? booking_account_id { get; set; }
    }

    public class BexioBillPayment
    {
        public string type { get; set; }
        public string iban { get; set; }
        public string reference_no { get; set; }
        public string note { get; set; }
        public string payment_date { get; set; }
    }

    /// <summary>Expense (api 4.0 expenses)</summary>
    public class BexioExpense
    {
        public string id { get; set; }
        public string document_no { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string currency_code { get; set; }
        public float? exchange_rate { get; set; }
        public float? amount { get; set; }
        public int? booking_account_id { get; set; }
        public int? tax_id { get; set; }
        public int? paid_on_account_id { get; set; }
        public string status { get; set; }
        public ICollection<string> attachment_ids { get; set; }
    }

    /// <summary>Outgoing payment (api 4.0 purchase/outgoing-payments)</summary>
    public class BexioOutgoingPayment
    {
        public string id { get; set; }
        public string bill_id { get; set; }
        public string status { get; set; }
        public string execution_date { get; set; }
        public float? amount { get; set; }
        public string currency_code { get; set; }
        public string sender_iban { get; set; }
        public string receiver_iban { get; set; }
        public string payment_type { get; set; }
        public string reference_no { get; set; }
        public string message { get; set; }
    }

    /// <summary>Payroll employee (api 4.0 payroll/employees)</summary>
    public class BexioEmployee
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }

    /// <summary>Payroll absence (api 4.0 payroll/absences)</summary>
    public class BexioAbsence
    {
        public string id { get; set; }
        public string employee_id { get; set; }
        public string absence_type { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public float? percentage { get; set; }
        public string comment { get; set; }
    }
}
