using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Helpers;
using Serilog;

namespace OffLogs.Api
{
    public class Program
    {    
        public static void Main(string[] args)
        {
            using var log = ApplicationHelper.BuildSerilogInstance();
            Log.Logger = log;
            
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog(Log.Logger)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(config =>
                {
                    config.ConfigureConfigurationProvider();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
