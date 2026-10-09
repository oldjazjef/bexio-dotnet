using bexio_lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;

namespace bexio_lib.Implementation
{
    public static class BexioServiceCollectionExtensions
    {
        /// <summary>
        /// Registers bexio with a typed HttpClient (IHttpClientFactory), options, IBexioClient,
        /// IBexioClientFactory and every single endpoint interface.
        /// </summary>
        /// <example>services.AddBexio(o => o.AccessToken = "...");</example>
        public static IServiceCollection AddBexio(this IServiceCollection services, Action<BexioOptions> configure)
        {
            services.AddOptions<BexioOptions>().Configure(configure);
            return services.AddBexioCore();
        }

        /// <summary>
        /// Binds <see cref="BexioOptions"/> from a configuration section, e.g. appsettings "Bexio": { "AccessToken": "..." }
        /// </summary>
        public static IServiceCollection AddBexio(this IServiceCollection services, IConfiguration section)
        {
            services.AddOptions<BexioOptions>().Bind(section);
            return services.AddBexioCore();
        }

        /// <summary>
        /// Legacy setup: reads "bexioApiUrl" (optional) and "bexioApiKey" from the configuration root.
        /// </summary>
        public static IServiceCollection AddBexioJwt(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddBexio(o =>
            {
                o.BaseUrl = configuration["bexioApiUrl"] ?? o.BaseUrl;
                o.AccessToken = configuration["bexioApiKey"];
            });
        }

        private static IServiceCollection AddBexioCore(this IServiceCollection services)
        {
            services.AddHttpClient(BexioApi.HttpClientName, (sp, http) =>
            {
                http.Timeout = sp.GetRequiredService<IOptions<BexioOptions>>().Value.Timeout;
            });

            services.TryAddTransient<IBexioApi>(sp => new BexioApi(
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(BexioApi.HttpClientName),
                sp.GetRequiredService<IOptions<BexioOptions>>()));
            services.TryAddSingleton<IBexioClientFactory, BexioClientFactory>();
            services.TryAddTransient<IBexioClient>(sp => new BexioClient(sp.GetRequiredService<IBexioApi>()));
            services.AddBexioEndpoints();
            return services;
        }

        /// <summary>
        /// Registers every endpoint (v2, v3, v4) of this assembly under its interface, e.g.
        /// IBexioApiInvoiceEndpoint -> BexioApiInvoiceEndpoint
        /// </summary>
        public static IServiceCollection AddBexioEndpoints(this IServiceCollection services)
        {
            var assembly = typeof(BexioServiceCollectionExtensions).Assembly;
            foreach (var implementation in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IBexioApiEndpoint).IsAssignableFrom(t)))
            {
                var service = implementation.GetInterfaces()
                    .FirstOrDefault(i => i.Name == "I" + implementation.Name && i.Assembly == assembly);
                if (service != null)
                {
                    services.TryAddTransient(service, implementation);
                }
            }
            return services;
        }
    }
}
