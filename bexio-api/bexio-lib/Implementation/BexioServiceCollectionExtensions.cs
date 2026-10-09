using bexio_lib.Interfaces;
using bexio_lib.OAuth;
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

        /// <summary>
        /// Authenticates with OAuth2 as an app registered at bexio instead of a personal access token.
        /// Call after <see cref="AddBexio(IServiceCollection, Action{BexioOptions})"/> (or without a token there).
        /// Registers <see cref="IBexioOAuthClient"/>, <see cref="IBexioTokenProvider"/> and, if not registered yet, an
        /// in-memory <see cref="IBexioTokenStore"/>: register your own store (database) before or after this call.
        /// </summary>
        public static IServiceCollection AddBexioOAuth(this IServiceCollection services, Action<BexioOAuthOptions> configure)
        {
            services.AddOptions<BexioOAuthOptions>().Configure(configure);
            return services.AddBexioOAuthCore();
        }

        /// <summary>Binds <see cref="BexioOAuthOptions"/> from a configuration section, e.g. "BexioOAuth"</summary>
        public static IServiceCollection AddBexioOAuth(this IServiceCollection services, IConfiguration section)
        {
            services.AddOptions<BexioOAuthOptions>().Bind(section);
            return services.AddBexioOAuthCore();
        }

        private static IServiceCollection AddBexioOAuthCore(this IServiceCollection services)
        {
            services.AddHttpClient(BexioOAuthClient.HttpClientName);
            services.TryAddSingleton(TimeProvider.System);
            services.TryAddSingleton<IBexioTokenStore, InMemoryBexioTokenStore>();
            services.TryAddTransient<IBexioOAuthClient>(sp => new BexioOAuthClient(
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(BexioOAuthClient.HttpClientName),
                sp.GetRequiredService<IOptions<BexioOAuthOptions>>(),
                sp.GetRequiredService<TimeProvider>()));
            // singleton: the refresh lock must be shared by all requests
            services.TryAddSingleton<IBexioTokenProvider>(sp => new BexioOAuthTokenProvider(
                sp.GetRequiredService<IBexioOAuthClient>(),
                sp.GetRequiredService<IBexioTokenStore>(),
                sp.GetRequiredService<IOptions<BexioOAuthOptions>>().Value,
                sp.GetRequiredService<TimeProvider>()));
            services.AddOptions<BexioOptions>().Configure<IBexioTokenProvider>((options, provider) =>
                options.AccessTokenProvider = provider.GetAccessTokenAsync);
            return services;
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
        /// Registers every endpoint (v2, v3) of this assembly under its interface, e.g.
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
