using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using Serilog;

namespace OffLogs.Api.Tests.Integration.Api.Frontend
{
    public class ApiFrontendCustomWebApplicationFactory: WebApplicationFactory<ApiFrontendTestStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // We can further customize our application setup here.
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // This method should be here to run the tests
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<OffLogs.Api.Frontend.Startup>()
                        .ConfigureTestServices(services => 
                        {
                            services.AddHttpContextAccessor();
                            services.InitDbServices();
                            services.AddScoped<IDataSeederService, DataSeederService>();
                            // We can further customize our application setup here.
                        })
                        .ConfigureAppConfiguration(builder =>
                        {
                            builder.ConfigureConfigurationProvider();
                        })
                        .UseSerilog()
                        .UseTestServer();
                });
            return builder;
        }
    }
}