using Autofac;
using OffLogs.Business.Orm.Connection;

namespace OffLogs.Business.Di.Autofac.Modules
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DbConnectionFactory>()
                .As<IDbConnectionFactory>()
                .SingleInstance();

            builder
                .RegisterType<DbSessionProvider>()
                .As<IDbSessionProvider>()
                .InstancePerLifetimeScope();
        }
    }
}