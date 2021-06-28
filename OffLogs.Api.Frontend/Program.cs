using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Helpers;
using Serilog;

namespace OffLogs.Api.Frontend
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
                .UseSerilog()
                .ConfigureAppConfiguration(config =>
                {
                    config.ConfigureConfigurationProvider();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}