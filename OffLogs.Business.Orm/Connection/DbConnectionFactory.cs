using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Utils;

namespace OffLogs.Business.Orm.Connection
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _dbNamespace = "OffLogs.Business.Db";
        private readonly IConfiguration _configuration;
        private ISessionFactory _sessionFactory;

        public DbConnectionFactory(IConfiguration configuration, ILogger<IDbConnectionFactory> logger)
        {
            this._configuration = configuration;
        }

        public Task<ISessionFactory> GetSessionFactoryAsync()
        {
            if (_sessionFactory == null)
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                var properties = new Dictionary<string, string>
                {
                    { "connection.connection_string", connectionString },
                    { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" }
                };
                var isShowSql = _configuration.GetValue<bool>("Hibernate:IsShowSql", false);
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
            return Task.FromResult(_sessionFactory);
        }

        public void Dispose()
        {
            _sessionFactory?.Close();
        }
    }
}