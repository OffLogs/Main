using System;
using System.Collections.Generic;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using OffLogs.Console.Core;
using OffLogs.Console.Verbs;
using Serilog;

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
                .AddLogging()
                .InitCommonServices()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IRunService, RunService>();
            
            serviceBuilder.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            var serviceProvider = serviceBuilder.BuildServiceProvider();
            
            return Parser.Default.ParseArguments<CreateNewUserVerb>(args)
                .MapResult(
                    (CreateNewUserVerb opts) =>
                    {
                        var runService = serviceProvider.GetService<IRunService>();
                        runService.RunStart();
                        return 0;
                    },
                    errs => 1);
        }
    }
}