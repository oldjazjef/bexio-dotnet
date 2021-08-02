using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    [Serializable]
    public class BexioApiException : Exception
    {
        public BexioApiException(string message) : base(string.IsNullOrEmpty(message) ? message : JsonConvert.DeserializeObject<BexioApiExceptionError>(message).Message)
        {
        }

    }

    public class BexioApiExceptionError
    {
        public string Message { get; set; }
    }
}
