using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Models
{
    public class BexioComment
    {
        public int id { get; set; }
        public string text { get; set; }
        public int user_id { get; set; }
        public string user_email { get; set; }
        public string user_name { get; set; }
        public DateTime date { get; set; }
        public bool is_public { get; set; }
        public byte[] image { get; set; }
        public string image_path { get; set; }

    }
}
