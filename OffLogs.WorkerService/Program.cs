using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using Serilog;
using Autofac.Extensions.DependencyInjection;

namespace OffLogs.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ApplicationHelper.BuildConfiguration();
            
            using var log = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureAppConfiguration(config =>
                {
                    config.ConfigureConfigurationProvider();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
