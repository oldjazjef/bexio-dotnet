using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioOrder
    {
        public int id { get; set; }
        public string document_nr { get; set; }
        public string title { get; set; }
        public int? contact_id { get; set; }
        public int? contact_sub_id { get; set; }
        public int? user_id { get; set; }
        public int? project_id { get; set; }
        public int? logopaper_id { get; set; }
        public int? language_id { get; set; }
        public int? bank_account_id { get; set; }
        public int? currency_id { get; set; }
        public int? payment_type_id { get; set; }
        public string header { get; set; }
        public string footer { get; set; }
        public float? total_gross { get; set; }
        public float? total_net { get; set; }
        public float? total_taxes { get; set; }
        public float? total { get; set; }
        public float? total_rounding_difference { get; set; }
        public int? mwst_type { get; set; }
        public bool mwst_is_net { get; set; }
        public bool show_position_taxes { get; set; }
        public DateTime? is_valid_from { get; set; }
        public string contact_address { get; set; }
        public string delivery_address_type { get; set; }
        public string delivery_address { get; set; }
        public int kb_item_status_id { get; set; }
        public bool is_recurring { get; set; }
        public string api_reference { get; set; }

        public DateTime? updated_at { get; set; }
        public string template_slug { get; set; }
        public string network_link { get; set; }
        public ICollection<BexioOrderTax> taxs { get; set; } = new List<BexioOrderTax>();
        public ICollection<BexioPosition> positions { get; set; } = new List<BexioPosition>();


        // manually mapped
        public ICollection<BexioItem> MappedArticles { get; set; } = new List<BexioItem>();
        public ICollection<BexioDiscountPosition> MappedDiscountPositions { get; set; } = new List<BexioDiscountPosition>();
        public BexioContact MappedDeliveryContact { get; set; }
        public ICollection<BexioComment> MappedComments { get; set; } = new List<BexioComment>();
    }

    public class BexioOrderTax
    {
        public float? percentage { get; set; }
        public float? value { get; set; }

    }

    public class BexioOrderDeliveryPosition
    {
        public int id { get; set; }
        public string type { get; set; }
        public float amount { get; set; }
    }

    public class BexioOrderInvoicePosition
    {
        public int id { get; set; }
        public string type { get; set; }
        public float amount { get; set; }
    }


    public class BexioOrderInvoiceUpdate
    {
        public ICollection<BexioOrderInvoicePosition> positions { get; set; }
    }

    public class BexioOrderDeliveryUpdate
    {
        public ICollection<BexioOrderDeliveryPosition> positions { get; set; }
    }

}
