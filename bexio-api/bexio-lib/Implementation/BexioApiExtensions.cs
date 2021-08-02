using bexio_lib.Implementation.Endpoints;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    public static class BexioApiExtensions
    {
        public static IServiceCollection UseJwt(this IServiceCollection services, IConfiguration configuration)
        {
    
            var bexioApi = BexioApi.UseJwt(
                configuration["apiUrl"],
                configuration["apiKey"]);

            services.AddSingleton<IBexioApi>(bexioApi);
            services.AddTransient<IBexioApiOrderEndpoint, BexioApiOrderEndpoint>();

            return services;
        }

        /// <summary>
        /// Extension method to add bexio specific query params and json body parameters to the api request
        /// </summary>
        /// <param name="request">Request object to operate on</param>
        /// <param name="requestParameters">Request parameter object</param>
        /// <returns></returns>
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
                    request.AddQueryParameter("order_by", string.Join(',', requestParameters.order_by));
                }

                if (requestParameters?.Filters.Any() == true)
                {
                    request.AddRequestBodyData(requestParameters?.Filters);
                }
            }
            return request;
        }

        /// <summary>
        /// Adds data as json to bexio request body
        /// </summary>
        /// <param name="request">The request to operate on</param>
        /// <param name="body">Data to add as json body</param>
        /// <returns></returns>
        public static RestRequest AddRequestBodyData(this RestRequest request, object body)
        {
            request.AddJsonBody(body);
            return request;
        }

        public static T DeserializeRequestResult<T>(this IRestResponse response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new BexioApiException(response.Content);
            }
            return JsonConvert.DeserializeObject<T>(response?.Content) ?? default;
        }

        /// <summary>
        /// Extension method to add new filter instruction to bexio request
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="instruction"></param>
        /// <returns></returns>
        public static BexioRequestFilter Add(this BexioRequestFilter filter, BexioRequestFilterInstruction instruction)
        {
            filter.Filters.Add(instruction);
            return filter;
        }

    }
}
