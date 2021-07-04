using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Utils;

namespace OffLogs.Business.Db.Dao
{
    public class BaseDao
    {
        private readonly string _dbNamespace = "OffLogs.Business.Db";
        
        protected ILogger<BaseDao> Logger;
        private static ISessionFactory _sessionFactory;
        
        protected ISession Session
        {
            get
            {
                return _sessionFactory.OpenSession();
            }
        }

        public BaseDao(IConfiguration configuration, ILogger<BaseDao> logger)
        {
            Logger = logger;
            if (_sessionFactory == null)
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var properties = new Dictionary<string, string>
                {
                    { "connection.connection_string", connectionString },
                    { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" }
                };
                var isShowSql = configuration.GetValue<bool>("Hibernate:IsShowSql", false);
                if (isShowSql)
                {
                    properties.Add("show_sql", "true");            
                    properties.Add("format_sql", "true");
                }

                var hibernateConfiguration = new Configuration()
                    .AddProperties(properties)
                    .Configure(currentAssembly, $"{_dbNamespace}.hibernate.hbm.xml");

                // Enable validation (optional)
                HbmSerializer.Default.Validate = true;

                // Import all entities and queries
                hibernateConfiguration.AddInputStream(HbmSerializer.Default.Serialize(currentAssembly));
                // Import all mapping files
                currentAssembly.GetManifestResourceNames().Where(resourceName => {
                    return resourceName.StartsWith($"{_dbNamespace}.Queries");
                    // TODO: Delete files and revert it
                    //|| resourceName.StartsWith($"{_dbNamespace}.Mapping")
                })
                .Select(resourceName => {
                    var filePath = IoUtils.GetResourcePath(currentAssembly, resourceName);
                    hibernateConfiguration.AddInputStream(currentAssembly.GetManifestResourceStream(filePath));
                    return resourceName;
                }).ToArray();

                _sessionFactory = hibernateConfiguration.BuildSessionFactory();
            }
        }

        public async Task<T> GetOneAsync<T>(object id)
        {
            using var session = Session;
            return await session.GetAsync<T>(id);
        }
    }
}