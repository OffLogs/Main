using Autofac;
using Commands.Abstractions;
using Notification.Abstractions;
using OffLogs.Business.Notifications;
using OffLogs.Business.Orm;
using OffLogs.Business.Orm.Commands;

namespace OffLogs.Business.Di.Autofac.Modules
{
    public class NotificationsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(BusinessNotificationsAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncNotification<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ScopedAsyncNotificationFactory>()
                .As<IAsyncNotificationFactory>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<DefaultAsyncNotificationBuilder>()
                .As<IAsyncNotificationBuilder>()
                .InstancePerLifetimeScope();
        }
    }
}