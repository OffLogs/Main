using Autofac;
using Domain.Abstractions;
using OffLogs.Business.Notifications;

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

            builder
                .RegisterAssemblyTypes(typeof(BusinessNotificationsAssemblyMarker).Assembly)
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}