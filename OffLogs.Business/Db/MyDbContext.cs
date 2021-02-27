using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OffLogs.Business.Db.Entity;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace OffLogs.Business.Db
{
    public class MyDbContext : DbContext
    {
        public DbSet<Log> Blogs { get; set; }
        public DbSet<User> Posts { get; set; }

        private readonly IConfiguration _configuration;
        
        public MyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(
                // Replace with your connection string.
                _configuration.GetConnectionString("DefaultConnection"),
                // Replace with your server version and type.
                // For common usages, see pull request #1233.
                new MySqlServerVersion(new Version(8, 0, 21)), // use MariaDbServerVersion for MariaDB
                mySqlOptions => mySqlOptions
                    .CharSetBehavior(CharSetBehavior.NeverAppend)
                );

        // Everything from this point on is optional but helps with debugging.
        // .EnableSensitiveDataLogging(true)
        // .EnableDetailedErrors(true);
    }
}