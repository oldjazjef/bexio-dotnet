namespace bexio_lib.Models
{
    public class BexioInvoicePayment
    {
        public int? id { get; set; }
        public string date { get; set; }
        public float? value { get; set; }
        public int? bank_account_id { get; set; }
        public int? payment_service_id { get; set; }
    }
}
