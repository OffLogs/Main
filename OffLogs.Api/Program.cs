using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace OffLogs.Api
{
    public class Program
    {
        private static string HostingEnvironment
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                return string.IsNullOrEmpty(value) ? "Development" : value;
            }
        }
        
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.${HostingEnvironment}.json", true)
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            
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
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{HostingEnvironment}.json", optional: true)
                        .AddJsonFile($"appsettings.Local.json", optional: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}