using Autofac;
using Domain.Abstractions;

namespace OffLogs.Business.Di.Autofac.Modules
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(BusinessAssemblyMarker).Assembly)
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}