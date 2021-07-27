using System.IO;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Api.Tests.Integration.Core.Faker;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Helpers;
using OffLogs.Business.Notifications.Core.Emails.Service;
using OffLogs.Business.Services.Data;
using Serilog;

namespace OffLogs.Api.Tests.Integration.Api.Main
{
    public class ApiCustomWebApplicationFactory: WebApplicationFactory<ApiTestStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<ApiTestStartup>()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureTestServices(services => 
                        {
                            services.AddHttpContextAccessor();
                            services.AddScoped<IDataFactoryService, DataFactoryService>();
                            services.AddScoped<IDataSeederService, DataSeederService>();
                            // We can further customize our application setup here.
                        })
                        .ConfigureAppConfiguration(builder =>
                        {
                            builder.ConfigureConfigurationProvider();
                        })
                        .UseTestServer();
                });
            return builder;
        }
    }
}