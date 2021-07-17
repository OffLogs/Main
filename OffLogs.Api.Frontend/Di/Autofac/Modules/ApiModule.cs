using Api.Requests.Abstractions;
using Autofac;

namespace OffLogs.Api.Frontend.Di.Autofac.Modules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ApiFrontendAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<>))
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(typeof(ApiFrontendAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>))
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