using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using OffLogs.Business.Common.Utils;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Email;

namespace OffLogs.Business.Helpers
{
    public static class ApplicationHelper
    {
        private static string HostingEnvironment
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                return string.IsNullOrEmpty(value) ? "Development" : value;
            }

        }
        
        public static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .ConfigureConfigurationProvider()
                .Build();
        }

        public static IConfigurationBuilder ConfigureConfigurationProvider(this IConfigurationBuilder builder)
        {
            var basePath = AssemblyUtils.GetAssemblyPath();
            return builder.SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.${HostingEnvironment}.json", true)
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddEnvironmentVariables();
        }
        
        public static Logger BuildSerilogInstance()
        {
            var configuration = BuildConfiguration();

            var logBuilder = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);

            var isSendEmailIfError = configuration.GetValue<bool>("Serilog:IsSendEmailIfError", false);
            if (isSendEmailIfError)
            {
                var connectionInfo = new EmailConnectionInfo()
                {
                    FromEmail = configuration.GetValue<string>("Smtp:From:Email"),
                    ToEmail = configuration.GetValue<string>("App:Email:Notification"),
                    MailServer = configuration.GetValue<string>("Smtp:Server"),
                    NetworkCredentials = new NetworkCredential(
                        configuration.GetValue<string>("Smtp:UserName"),
                        configuration.GetValue<string>("Smtp:Password")
                    ),
                    EmailSubject =
                        "OffLogs System Error: {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}",
                    EnableSsl = configuration.GetValue<bool>("Smtp:EnableSsl"),
                    Port = configuration.GetValue<int>("Smtp:Port")
                };
                logBuilder.WriteTo.Email(connectionInfo, restrictedToMinimumLevel: LogEventLevel.Verbose);
            }
            
            return logBuilder.CreateLogger();
        }
    }
}
