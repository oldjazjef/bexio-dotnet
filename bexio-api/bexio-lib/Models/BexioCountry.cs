using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioCountry
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_short { get; set; }
        public string iso3166_alpha2 { get; set; }
    }
}
