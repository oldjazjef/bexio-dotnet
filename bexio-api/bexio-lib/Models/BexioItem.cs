using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioItem
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public int? article_type_id { get; set; }
        public int? contact_id { get; set; }
        public string deliverer_code { get; set; }
        public string deliverer_name { get; set; }
        public string deliverer_description { get; set; }
        public string intern_code { get; set; }
        public string intern_name { get; set; }
        public string intern_description { get; set; }
        public float? purchase_price { get; set; }
        public float? sale_price { get; set; }
        public float? purchase_total { get; set; }
        public float? sale_total { get; set; }
        public int? currency_id { get; set; }
        public int? tax_income_id { get; set; }
        public int? tax_id { get; set; }
        public int? tax_expense_id { get; set; }
        public int? unit_id { get; set; }
        public bool is_stock { get; set; }
        public int? stock_id { get; set; }
        public int? stock_place_id { get; set; }
        public int? stock_nr { get; set; }
        public int? stock_min_nr { get; set; }
        public int? stock_reserved_nr { get; set; }
        public int? stock_available_nr { get; set; }
        public int? stock_picked_nr { get; set; }
        public int? stock_disposed_nr { get; set; }
        public int? stock_ordered_nr { get; set; }
        public object width { get; set; }
        public object height { get; set; }
        public object weight { get; set; }
        public object volume { get; set; }
        public string html_text { get; set; }
        public string remarks { get; set; }
        public float? delivery_price { get; set; }
        public int? article_group_id { get; set; }
    }
}
