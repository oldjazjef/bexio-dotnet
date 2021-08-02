using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioPosition
    {
        public int id { get; set; }
        public float? amount { get; set; }
        public int? unit_id { get; set; }
        public int? account_id { get; set; }
        public string unit_name { get; set; }
        public int? tax_id { get; set; }
        public float? tax_value { get; set; }
        public string text { get; set; }
        public float? unit_price { get; set; }
        public float? discount_in_percent { get; set; }
        public float? position_total { get; set; }
        public int? pos { get; set; }
        public int? pos_internal { get; set; }
        public bool is_optional { get; set; }
        public int? article_id { get; set; }
        public string type { get; set; }
        public int? parent_id { get; set; }
    }

}
