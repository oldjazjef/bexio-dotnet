using bexio_lib.Interfaces;
using bexio_lib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace bexio_lib.Implementation
{
    public static class BexioApiExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Adds limit / offset / order_by query parameters
        /// </summary>
        public static BexioRequest AddRequestData(this BexioRequest request, BexioRequestFilter requestParameters)
        {
            if (requestParameters != null)
            {
                if (requestParameters.limit != null)
                {
                    request.AddQueryParameter("limit", requestParameters.limit.ToString());
                }

                if (requestParameters.offset != null)
                {
                    request.AddQueryParameter("offset", requestParameters.offset.ToString());
                }

                if (requestParameters.order_by?.Any() == true)
                {
                    request.AddQueryParameter("order_by", string.Join(",", requestParameters.order_by));
                }
            }
            return request;
        }

        /// <summary>
        /// Adds query parameters and the filter instructions as json body (search endpoints)
        /// </summary>
        public static BexioRequest AddSearchData(this BexioRequest request, BexioRequestFilter requestParameters)
        {
            request.AddRequestData(requestParameters);
            return request.AddRequestBodyData((object)(requestParameters?.Filters ?? new List<BexioRequestFilterInstruction>()));
        }

        /// <summary>
        /// Adds data as json to bexio request body. Null properties are omitted.
        /// </summary>
        public static BexioRequest AddRequestBodyData(this BexioRequest request, object body)
        {
            return request.AddJsonBody(JsonConvert.SerializeObject(body, SerializerSettings));
        }

        private static bool IsSuccess(BexioResponse response)
            => (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;

        /// <summary>
        /// Throws a <see cref="BexioApiException"/> if the response is not a 2xx
        /// </summary>
        public static BexioResponse EnsureSuccess(this BexioResponse response)
        {
            if (!IsSuccess(response))
            {
                var content = string.IsNullOrEmpty(response.Content) ? response.ErrorMessage : response.Content;
                throw new BexioApiException(response.StatusCode == 0 ? (HttpStatusCode?)null : response.StatusCode, content);
            }
            return response;
        }

        public static T DeserializeRequestResult<T>(this BexioResponse response)
        {
            response.EnsureSuccess();
            if (string.IsNullOrWhiteSpace(response.Content))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        /// <summary>
        /// Delete answers either with 204, or 200 and {"success": true}
        /// </summary>
        public static bool ToSuccessResult(this BexioResponse response) => response.ToDeleteResult();

        public static bool ToDeleteResult(this BexioResponse response)
        {
            response.EnsureSuccess();
            if (string.IsNullOrWhiteSpace(response.Content))
            {
                return true;
            }
            try
            {
                if (!(JToken.Parse(response.Content) is JObject obj))
                {
                    return true;
                }
                var success = obj["success"];
                return success == null || success.Type != JTokenType.Boolean || (bool)success;
            }
            catch (JsonException)
            {
                return true;
            }
        }

        /// <summary>
        /// Extension method to add new filter instruction to bexio request
        /// </summary>
        public static BexioRequestFilter Add(this BexioRequestFilter filter, BexioRequestFilterInstruction instruction)
        {
            filter.Filters.Add(instruction);
            return filter;
        }
    }
}
