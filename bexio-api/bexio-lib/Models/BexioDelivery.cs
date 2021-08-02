using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioDelivery
    {
        public int id { get; set; }
        public string document_nr { get; set; }
        public string api_reference { get; set; }
        public string kb_item_status_id { get; set; }
    }
}
