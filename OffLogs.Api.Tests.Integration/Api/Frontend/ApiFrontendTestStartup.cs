using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Frontend.Di.Autofac.Modules;
using OffLogs.Business.Di.Autofac.Modules;

namespace OffLogs.Api.Tests.Integration.Api.Frontend
{
    public class ApiFrontendTestStartup: OffLogs.Api.Frontend.Startup
    {
        public ApiFrontendTestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterModule<ApiModule>()
                .RegisterModule<DomainModule>()
                .RegisterModule<DbModule>()
                .RegisterModule<CommandsModule>()
                .RegisterModule<QueriesModule>();
        }
    }
}