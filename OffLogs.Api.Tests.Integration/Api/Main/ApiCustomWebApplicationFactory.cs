using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Helpers;
using Serilog;

namespace OffLogs.Api.Tests.Integration.Api.Main
{
    public class ApiCustomWebApplicationFactory: WebApplicationFactory<ApiTestStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove hosted services
                
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    // Vizit.Migration.Program.Migrate(configuration);                    
                }
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
                    builder.UseStartup<Startup>()
                        .ConfigureTestServices(services => 
                        {
                            services.AddHttpContextAccessor();
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