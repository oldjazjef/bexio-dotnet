using bexio_lib.Implementation;
using bexio_lib.Implementation.Endpoints;
using bexio_lib.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BexioLibTest
{
    public class Startup
    {

        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureHostConfiguration(builder => {
                    builder
                    .AddEnvironmentVariables()
                    .Build();
                })
                .ConfigureAppConfiguration((context, builder) => {
                    builder
                    .SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", false, false)
                    .AddJsonFile("apisecrets.json", false, false)
                    .AddEnvironmentVariables()
                    .Build();
                });

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {

            var bexioApi = BexioApi.UseJwt(
                context.Configuration["apiUrl"],
                context.Configuration["apiKey"]);

            services.AddSingleton<IBexioApi>(bexioApi);
            services.AddTransient<IBexioApiOrderEndpoint, BexioApiOrderEndpoint>();
        }
    }
}
