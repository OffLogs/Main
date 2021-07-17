using Autofac;
using Commands.Abstractions;
using OffLogs.Business.Orm;
using OffLogs.Business.Orm.Commands;
using OffLogs.Business.Orm.Connection;
using OffLogs.Business.Orm.Queries;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Di.Autofac.Modules
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