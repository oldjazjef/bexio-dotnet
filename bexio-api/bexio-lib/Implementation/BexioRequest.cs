using System.Collections.Generic;

namespace bexio_lib.Implementation
{
    /// <summary>
    /// Transport independent description of a request to the bexio api.
    /// </summary>
    public class BexioRequest
    {
        public BexioRequest(string resource)
        {
            this.Resource = resource;
        }

        /// <summary>Path relative to the api base url, e.g. "2.0/contact/5"</summary>
        public string Resource { get; }

        public List<KeyValuePair<string, string>> Query { get; } = new List<KeyValuePair<string, string>>();

        /// <summary>Json body, null if the request has none</summary>
        public string JsonBody { get; private set; }

        /// <summary>Single file to send as multipart/form-data</summary>
        public BexioUpload Upload { get; private set; }

        /// <summary>Accept header, defaults to application/json</summary>
        public string Accept { get; set; } = "application/json";

        public BexioRequest AddQueryParameter(string name, string value)
        {
            this.Query.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public BexioRequest AddJsonBody(string json)
        {
            this.JsonBody = json;
            return this;
        }

        public BexioRequest AddFile(string fieldName, string fileName, byte[] content)
        {
            this.Upload = new BexioUpload(fieldName, fileName, content);
            return this;
        }
    }

    public class BexioUpload
    {
        public BexioUpload(string fieldName, string fileName, byte[] content)
        {
            this.FieldName = fieldName;
            this.FileName = fileName;
            this.Content = content;
        }

        public string FieldName { get; }
        public string FileName { get; }
        public byte[] Content { get; }
    }

    public class BexioResponse
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public byte[] RawBytes { get; set; }
        /// <summary>Set if no response was received at all (network error, timeout)</summary>
        public string ErrorMessage { get; set; }
    }
}
