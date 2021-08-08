using Autofac;
using Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Di.Autofac.Modules;
using OffLogs.Api.Tests.Integration.Core.Faker;
using OffLogs.Business;
using OffLogs.Business.Di.Autofac.Modules;
using OffLogs.Business.Notifications.Services;
using OffLogs.Business.Services.Http.ThrottleRequests;

namespace OffLogs.Api.Tests.Integration.Api.Main
{
    public class ApiTestStartup: OffLogs.Api.Startup
    {
        public ApiTestStartup(IConfiguration configuration) : base(configuration)
        {
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterModule<ApiModule>()
                .RegisterModule<DbModule>()
                .RegisterModule<QueriesModule>()
                .RegisterModule<CommandsModule>()
                .RegisterModule<NotificationsModule>();

            builder
                .RegisterType<ThrottleRequestsService>()
                .As<IThrottleRequestsService>()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(typeof(BusinessAssemblyMarker).Assembly)
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // Register fakes
            builder
                .RegisterType<FakeEmailSendingService>()
                .As<IEmailSendingService>()
                .InstancePerLifetimeScope();
        }
    }
}