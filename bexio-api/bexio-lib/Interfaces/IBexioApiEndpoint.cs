using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    public interface IBexioApiEndpoint
    {
        public string ENDPOINT { get; init; }
        public IBexioApi API { get; init; }
    }
}
