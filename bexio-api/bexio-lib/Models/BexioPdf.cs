using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioPdf
    {
        public string name { get; set; }
        public int size { get; set; }
        public string mime { get; set; }
        public byte[] content { get; set; }
    }
}
