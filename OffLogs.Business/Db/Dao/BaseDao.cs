using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Cfg;
namespace OffLogs.Business.Db.Dao
{
    public class BaseDao
    {
        protected ILogger<BaseDao> Logger;
        
        private static ISessionFactory _sessionFactory;
        
        protected ISession Session
        {
            get
            {
                return _sessionFactory.OpenSession();
            }
        }

        public BaseDao(string connString, ILogger<BaseDao> logger)
        {
            Logger = logger;
            if (_sessionFactory == null)
            {
                var properties = new Dictionary<string, string>()
                {
                    { "connection.connection_string", connString },
                    { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" }
                };
                
#if DEBUG
                properties.Add("show_sql", "true");            
                properties.Add("format_sql", "true");
#endif
                
                _sessionFactory = new Configuration()
                    .Configure(Assembly.GetExecutingAssembly(), "OffLogs.Business.Db.hibernate.cfg.xml")
                    .SetProperties(properties)
                    .BuildSessionFactory();
            }
        }

        public async Task<T> GetOneAsync<T>(object id)
        {
            using var session = Session;
            return await session.GetAsync<T>(id);
        }
    }
}