using Autofac;
using OffLogs.Business.Orm;
using OffLogs.Business.Orm.Queries;
using Queries.Abstractions;

namespace OffLogs.Business.Di.Autofac.Modules
{
    public class QueriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(FindObjectWithIdByIdQuery<>))
                .As(typeof(IAsyncQuery<,>))
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(BusinessOrmAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IAsyncQuery<,>))
                .InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(DefaultAsyncQueryFor<>))
                .As(typeof(IAsyncQueryFor<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ScopedAsyncQueryFactory>()
                .As<IAsyncQueryFactory>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ScopedAsyncQueryBuilder>()
                .As<IAsyncQueryBuilder>()
                .InstancePerLifetimeScope();
        }
    }
}