using System;
using System.Collections.Generic;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using OffLogs.Console.Core;
using OffLogs.Console.Verbs;
using Serilog;
using Serilog.Extensions.Logging;

namespace OffLogs.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            var configuration = ApplicationHelper.BuildConfiguration();
            
            using var log = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            Log.Logger = log;
            
            var serviceBuilder = new ServiceCollection()
                .InitCommonServices()
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<ICreateUserService, CreateUserService>()
                .AddScoped<ICreateApplicationService, CreateApplicationService>();

            serviceBuilder.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            var serviceProvider = serviceBuilder.BuildServiceProvider();
            
            return Parser.Default.ParseArguments<CreateNewUserVerb, CreateNewApplicationVerb, int>(args)
                .MapResult(
                    (CreateNewUserVerb verb) =>
                    {
                        var runService = serviceProvider.GetService<ICreateUserService>();
                        return runService.CreateUser(verb).Result;
                    },
                    (CreateNewApplicationVerb verb) =>
                    {
                        var runService = serviceProvider.GetService<ICreateApplicationService>();
                        return runService.Create(verb).Result;
                    },
                    errs =>
                    {
                        foreach (var error in errs)
                        {
                            log.Error(error.ToString());
                        }
                        return 1;
                    }
                );
        }
    }
}