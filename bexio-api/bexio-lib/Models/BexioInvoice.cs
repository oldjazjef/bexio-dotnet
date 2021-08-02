using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioInvoice
    {
        public int id { get; set; }
        public string document_nr { get; set; }
        public string title { get; set; }
        public int contact_id { get; set; }
        public int? contact_sub_id { get; set; }
        public int user_id { get; set; }
        public int? project_id { get; set; }
        public int? logopaper_id { get; set; }
        public int? language_id { get; set; }
        public int? bank_account_id { get; set; }
        public int? currency_id { get; set; }
        public string? currency_name { get; set; }
        public int? payment_type_id { get; set; }
        public string header { get; set; }
        public string footer { get; set; }
        public string total_gross { get; set; }
        public string total_net { get; set; }
        public string total_taxes { get; set; }
        public string total_received_payments { get; set; }
        public string total_credit_vouchers { get; set; }
        public string total_remaining_payments { get; set; }
        public string total { get; set; }
        public double total_rounding_difference { get; set; }
        public int? mwst_type { get; set; }
        public bool mwst_is_net { get; set; }
        public bool show_position_taxes { get; set; }
        public string is_valid_from { get; set; }
        public string is_valid_to { get; set; }
        public string contact_address { get; set; }
        public int kb_item_status_id { get; set; }
        public string reference { get; set; }
        public string api_reference { get; set; }
        public DateTime? viewed_by_client_at { get; set; }
        public string updated_at { get; set; }
        public int? esr_id { get; set; }
        public int? qr_invoice_id { get; set; }
        public string template_slug { get; set; }
        public string network_link { get; set; }

        public ICollection<BexioPosition> positions { get; set; }
    }

    public class BexioInvoiceSend
    {
        public string recipient_email { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public bool mark_as_open { get; set; }
    }

}
