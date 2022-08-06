using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OffLogs.Business.Helpers;
using Serilog;

namespace OffLogs.Api.Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var log = ApplicationHelper.BuildSerilogInstance();
            Log.Logger = log;

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                log.Error(e, "Start application failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(Log.Logger)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(config =>
                {
                    config.ConfigureConfigurationProvider();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
