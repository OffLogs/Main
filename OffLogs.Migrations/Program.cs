using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Migrations.Migrations;

namespace OffLogs.Migrations
{
    public class Program
    {
        private static string HostingEnvironment {
            get {
                var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                return string.IsNullOrEmpty(value) ? "Development" : value;
            }
        }

        private static string CustomConnectionString => Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING");

        public static IConfiguration Configuration;
        public static string ConnectionString;

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json", true)
                .AddEnvironmentVariables()
                .Build();
            ConnectionString = string.IsNullOrEmpty(CustomConnectionString)
                ? Configuration.GetConnectionString("DefaultConnection")
                : CustomConnectionString;

            Migrate(ConnectionString);

            Console.WriteLine("Migrations are applied...");
#if DEBUG
            Console.ReadKey();
#endif
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        public static void Migrate(string connectionString)
        {
            var migrationClasses = (
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.Namespace.Contains("OffLogs.Migrations")
                select t.Assembly
            );

            var serviceProvider = new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSqlServer()
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