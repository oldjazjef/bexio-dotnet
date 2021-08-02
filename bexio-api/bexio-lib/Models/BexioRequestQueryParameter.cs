using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioRequestQueryParameter
    {
        public string[]? order_by { get; set; }
        public int? offset { get; set; }
        public int? limit { get; set; }
    }

    public class BexioRequestFilter : BexioRequestQueryParameter
    {
        public ICollection<BexioRequestFilterInstruction> Filters { get; init; } 

        public BexioRequestFilter()
        {
           this.Filters = new List<BexioRequestFilterInstruction>();
        }
    }

    public class BexioRequestFilterInstruction
    {
        public string field { get; init; }
        public object value { get; init; }
        public string criteria { get; init; }

        public BexioRequestFilterInstruction(string field, object value, string criteria)
        {
            this.field = field;
            this.value = value;
            this.criteria = criteria;
        }
    }

}
