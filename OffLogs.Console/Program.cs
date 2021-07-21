using CommandLine;
using Microsoft.Extensions.Configuration;
using OffLogs.Business.Helpers;
using OffLogs.Console.Core;
using OffLogs.Console.Verbs;
using Autofac;
using OffLogs.Business;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace OffLogs.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            var configuration = ApplicationHelper.BuildConfiguration();
            using var serviceProvider = BuildDiContainer(configuration);
            using var scope = serviceProvider.BeginLifetimeScope();

            var log = scope.Resolve<ILogger<Program>>();

            return Parser.Default.ParseArguments<CreateNewUserVerb, CreateNewApplicationVerb, int>(args)
                .MapResult(
                    (CreateNewUserVerb verb) =>
                    {
                        var runService = scope.Resolve<ICreateUserService>();
                        return runService.CreateUser(verb).Result;
                    },
                    (CreateNewApplicationVerb verb) =>
                    {
                        var runService = scope.Resolve<ICreateApplicationService>();
                        return runService.Create(verb).Result;
                    },
                    errs =>
                    {
                        foreach (var error in errs)
                        {
                            log.LogError(error.ToString());
                        }
                        return 1;
                    }
                );
        }

        private static IContainer BuildDiContainer(IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(configuration)
                .As<IConfiguration>()
                .SingleInstance();

            var serilogConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);
            builder.RegisterSerilog(serilogConfiguration);

            builder.RegisterType<CreateUserService>()
                .As<ICreateUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreateApplicationService>()
                .As<ICreateApplicationService>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyModules(typeof(Program).Assembly);
            builder.RegisterAssemblyModules(typeof(BusinessAssemblyMarker).Assembly);

            return builder.Build();
        }
    }
}