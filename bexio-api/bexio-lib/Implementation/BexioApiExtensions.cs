using bexio_lib.Interfaces;
using bexio_lib.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace bexio_lib.Implementation
{
    public static class BexioApiExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Registers the api and all endpoints. Reads "bexioApiUrl" (optional) and "bexioApiKey" from the configuration.
        /// </summary>
        public static IServiceCollection AddBexioJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var bexioApi = BexioApi.UseJwt(
                configuration["bexioApiUrl"],
                configuration["bexioApiKey"]);

            services.AddSingleton<IBexioApi>(bexioApi);
            services.AddBexioEndpoints();

            return services;
        }

        /// <summary>
        /// Registers every endpoint (v2 and v3) of this assembly under its interface, e.g.
        /// IBexioApiInvoiceEndpoint -> BexioApiInvoiceEndpoint
        /// </summary>
        public static IServiceCollection AddBexioEndpoints(this IServiceCollection services)
        {
            var assembly = typeof(BexioApiExtensions).Assembly;
            foreach (var implementation in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IBexioApiEndpoint).IsAssignableFrom(t)))
            {
                var service = implementation.GetInterfaces()
                    .FirstOrDefault(i => i.Name == "I" + implementation.Name && i.Assembly == assembly);
                if (service != null)
                {
                    services.AddTransient(service, implementation);
                }
            }
            return services;
        }

        /// <summary>
        /// Adds limit / offset / order_by query parameters
        /// </summary>
        public static RestRequest AddRequestData(this RestRequest request, BexioRequestFilter requestParameters)
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
        public static RestRequest AddSearchData(this RestRequest request, BexioRequestFilter requestParameters)
        {
            request.AddRequestData(requestParameters);
            return request.AddRequestBodyData((object)(requestParameters?.Filters ?? new List<BexioRequestFilterInstruction>()));
        }

        /// <summary>
        /// Adds data as json to bexio request body. Null properties are omitted.
        /// </summary>
        public static RestRequest AddRequestBodyData(this RestRequest request, object body)
        {
            request.AddParameter("application/json", JsonConvert.SerializeObject(body, SerializerSettings), ParameterType.RequestBody);
            return request;
        }

        private static bool IsSuccess(IRestResponse response)
            => (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;

        /// <summary>
        /// Throws a <see cref="BexioApiException"/> if the response is not a 2xx
        /// </summary>
        public static IRestResponse EnsureSuccess(this IRestResponse response)
        {
            if (!IsSuccess(response))
            {
                var content = string.IsNullOrEmpty(response.Content) ? response.ErrorMessage : response.Content;
                throw new BexioApiException(response.StatusCode == 0 ? (HttpStatusCode?)null : response.StatusCode, content);
            }
            return response;
        }

        public static T DeserializeRequestResult<T>(this IRestResponse response)
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
        public static bool ToDeleteResult(this IRestResponse response)
        {
            response.EnsureSuccess();
            if (string.IsNullOrWhiteSpace(response.Content))
            {
                return true;
            }
            try
            {
                var success = JToken.Parse(response.Content)["success"];
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
