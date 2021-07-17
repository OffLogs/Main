using Autofac;
using Commands.Abstractions;
using OffLogs.Business.Orm;
using OffLogs.Business.Orm.Commands;

namespace OffLogs.Business.Di.Autofac.Modules
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(CreateObjectWithIdCommand<>))
                .As(typeof(IAsyncCommand<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(BusinessOrmAssemblyHandler).Assembly)
                .AsClosedTypesOf(typeof(IAsyncCommand<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ScopedAsyncCommandFactory>()
                .As<IAsyncCommandFactory>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<DefaultAsyncCommandBuilder>()
                .As<IAsyncCommandBuilder>()
                .InstancePerLifetimeScope();
        }
    }
}