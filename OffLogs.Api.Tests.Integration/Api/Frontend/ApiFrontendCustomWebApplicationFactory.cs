using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Di.Autofac.Modules;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using OffLogs.Business.Services.Data;
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<OffLogs.Api.Frontend.Startup>()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureTestServices(services => 
                        {
                            services.AddScoped<IDataFactoryService, DataFactoryService>();
                            services.AddScoped<IDataSeederService, DataSeederService>();
                            // We can further customize our application setup here.
                        })
                        .ConfigureTestContainer<Autofac.ContainerBuilder>(builder => {
                            // called after Startup.ConfigureContainer
                            //builder.RegisterModule<DomainModule>();
                            //builder.RegisterModule<DbModule>();
                            //builder.RegisterModule<CommandsModule>();
                            //builder.RegisterModule<QueriesModule>();
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