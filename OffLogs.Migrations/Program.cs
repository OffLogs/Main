using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Common.Utils;
using OffLogs.Migrations.Migrations;

namespace OffLogs.Migrations
{
    public class Program
    {
        private static string CustomConnectionString => Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING");

        public static IConfiguration Configuration;
        
        static void Main(string[] args)
        {
            var basePath = AssemblyUtils.GetAssemblyPath();
            Configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json", true)
                .AddEnvironmentVariables()
                .Build();
            
            var defaultConnectionString = string.IsNullOrEmpty(CustomConnectionString)
                ? Configuration.GetConnectionString("DefaultConnection")
                : CustomConnectionString;
            Migrate(defaultConnectionString);

            var testConnectionString = Configuration.GetConnectionString("TestConnection");
            if (!string.IsNullOrEmpty(testConnectionString))
            {
                Migrate(testConnectionString);    
            }
            Console.WriteLine("Migrations are applied...");
            Console.ReadKey();
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        public static void Migrate(string connectionString)
        {
            var migrationClasses = (
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass 
                    && t.Namespace != null 
                    && t.Namespace.Contains("OffLogs.Migrations")
                select t.Assembly
            );

            var serviceProvider = new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(migrationClasses.ToArray()).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Update the database
        /// </sumamry>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            // Execute the migrations
            runner.MigrateUp();
            runner.Up(new ApplyProceduresMigration());
        }
    }
}
