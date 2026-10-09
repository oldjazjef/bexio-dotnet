using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace bexio_lib.Implementation
{
    [Serializable]
    public class BexioApiException : Exception
    {
        public HttpStatusCode? StatusCode { get; }
        public string Content { get; }

        public BexioApiException(string message) : this(null, message)
        {
        }

        public BexioApiException(HttpStatusCode? statusCode, string content)
            : base(ExtractMessage(statusCode, content))
        {
            this.StatusCode = statusCode;
            this.Content = content;
        }

        private static string ExtractMessage(HttpStatusCode? statusCode, string content)
        {
            var prefix = statusCode.HasValue ? $"{(int)statusCode.Value} {statusCode.Value}" : null;
            string message = null;
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    var token = JToken.Parse(content);
                    if (token is JObject obj)
                    {
                        message = (string)obj["message"] ?? (string)obj["error"] ?? (string)obj["error_message"];
                        if (obj["errors"] is JArray errors && errors.Count > 0)
                        {
                            message = $"{message} {errors.ToString(Newtonsoft.Json.Formatting.None)}".Trim();
                        }
                    }
                }
                catch (Exception)
                {
                    // not json, use raw content below
                }
                message ??= content;
            }
            if (prefix != null)
            {
                return message == null ? prefix : $"{prefix}: {message}";
            }
            return message ?? "Unknown bexio api error";
        }
    }
}
