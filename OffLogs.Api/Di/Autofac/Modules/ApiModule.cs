using Api.Requests.Abstractions;
using Autofac;

namespace OffLogs.Api.Di.Autofac.Modules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ApiAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<>))
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(typeof(ApiAssemblyMarker).Assembly)
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