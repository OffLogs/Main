using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace OffLogs.Api.Tests.Integration
{
    public class CustomWebApplicationFactory: WebApplicationFactory<TestStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json", true)
                .Build();
            
            builder.ConfigureServices(services =>
            {
                // var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IViberCommunicationService));
                // services.Remove(descriptor);
                // services.AddSingleton<IViberCommunicationService, TestViberCommunicationService>();

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
                        .ConfigureAppConfiguration(builder =>
                        {
                            builder.AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.Local.json", optional: true)
                                .AddEnvironmentVariables();
                        })
                        .ConfigureTestServices(services => 
                        {
                            // We can further customize our application setup here.
                        })
                        .UseSerilog()
                        .UseTestServer();
                });
            return builder;
        }
    }
}