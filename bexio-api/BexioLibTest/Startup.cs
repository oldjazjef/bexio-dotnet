using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BexioLibTest
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("apisettings.json", true, false)
                        .AddJsonFile("apisecrets.json", true, false)
                        .AddEnvironmentVariables();
                });

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            // Integration tests use the real api, unit tests use the FakeBexioApi directly.
            if (string.IsNullOrEmpty(context.Configuration["bexioApiKey"]))
            {
                // no credentials: register with a dummy token, integration tests then skip themselves
                context.Configuration["bexioApiKey"] = "not-configured";
            }
            services.AddBexioJwt(context.Configuration);
        }
    }
}
