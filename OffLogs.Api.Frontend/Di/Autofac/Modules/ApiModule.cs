using Api.Requests.Abstractions;
using Autofac;
using Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Frontend.Di.Autofac.Modules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RequestService>()
                .As<IRequestService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<JwtApplicationService>()
                .As<IJwtApplicationService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<JwtAuthService>()
                .As<IJwtAuthService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ThrottleRequestsService>()
                .As<IThrottleRequestsService>()
                .SingleInstance();

            builder
                .RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<LogAssembler>()
                .As<ILogAssembler>()
                .InstancePerLifetimeScope();
            
            builder
                .RegisterAssemblyTypes(typeof(ApiFrontendAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<>))
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(typeof(ApiFrontendAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>))
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(typeof(ApiFrontendAssemblyMarker).Assembly)
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
            
            builder
                .RegisterType<ScopedAsyncRequestHandlerFactory>()
                .As<IAsyncRequestHandlerFactory>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<DefaultAsyncRequestBuilder>()
                .As<IAsyncRequestBuilder>()
                .InstancePerLifetimeScope();
        }
    }
}
