using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioDiscountPosition
    {
        public int id { get; set; }
        public string text { get; set; }
        public bool is_percentual { get; set; }
        public float? value { get; set; }
        public float? discount_total { get; set; }
        public string type { get; set; }
    }
}
