using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Multi;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Dao
{
    public class LogDao: CommonDao, ILogDao
    {
        public LogDao(
            IConfiguration configuration, 
            Microsoft.Extensions.Logging.ILogger<LogDao> logger
        ) : base(
            configuration,
            logger
        )
        {
        }

        public async Task<LogEntity> GetLogAsync(string token)
        {
            using (var session = Session)
            {
                var log = await session.Query<LogEntity>()
                    .Fetch(record => record.Application)
                    .Where(e => e.Token == token)
                    .FirstOrDefaultAsync();
                if (log != null)
                {
                    var tracesQuery = session.Query<LogTraceEntity>()
                        .Where(record => record.Log.Id == log.Id);
                    var propertiesQuery = session.Query<LogPropertyEntity>()
                        .Where(record => record.Log.Id == log.Id);

                    var queries = session.CreateQueryBatch()
                        .Add("traces", tracesQuery)
                        .Add("properties", propertiesQuery);

                    var traces = queries.GetResult<LogTraceEntity>("traces")
                        .ToList();
                    foreach (var trace in traces)
                        log.AddTrace(trace);
                    var properties = queries.GetResult<LogPropertyEntity>("properties").ToList();
                    foreach (var property in properties)
                        log.AddProperty(property);
                }
                
                return await Task.FromResult(log);    
            }
        }
        
        public async Task<LogEntity> GetLogAsync(long logId)
        {
            using (var session = Session)
            {
                var logQuery = session.Query<LogEntity>()
                    .Where(record => record.Id == logId)
                    .Fetch(record => record.Application);
                var tracesQuery = session.Query<LogTraceEntity>()
                    .Where(record => record.Log.Id == logId);
                var propertiesQuery = session.Query<LogPropertyEntity>()
                    .Where(record => record.Log.Id == logId);

                var queries = session.CreateQueryBatch()
                    .Add("log", logQuery)
                    .Add("traces", tracesQuery)
                    .Add("properties", propertiesQuery);

                var log = queries.GetResult<LogEntity>("log").SingleOrDefault();
                if (log != null)
                {
                    log.Traces = queries.GetResult<LogTraceEntity>("traces").ToList();
                    log.Properties = queries.GetResult<LogPropertyEntity>("properties").ToList();
                }
                return await Task.FromResult(log);    
            }
        }

        public async Task<LogEntity> AddAsync(LogEntity log)
        {
            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                // Clear bags
                var properties = log.Properties.ToList();
                log.Properties = new List<LogPropertyEntity>();
                var traces = log.Traces.ToList();
                log.Traces = new List<LogTraceEntity>();
                
                log.Id = (long)await session.SaveAsync(log);

                properties.ForEach(log.AddProperty);
                traces.ForEach(log.AddTrace);
                
                await session.UpdateAsync(log);
                await transaction.CommitAsync();
            }
            return log;
        }
        
        public async Task<LogEntity> AddAsync(
            ApplicationEntity application,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        )
        {
            var log = new LogEntity()
            {
                Application = application,
                Message = message,
                Level = level,
                LogTime = timestamp
            };
            if (properties != null)
            {
                foreach (var logPropertyEntity in properties)
                {
                    log.AddProperty(logPropertyEntity);
                }
            }
            if (traces != null)
            {
                foreach (var logTraceEntity in traces)
                {
                    log.AddTrace(logTraceEntity);
                }
            }

            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                log.Id = (long)await session.SaveAsync(log);
                await transaction.CommitAsync();
            }
            return log;
        }
        
        public async Task<(IEnumerable<LogEntity>, long)> GetList(
            long applicationId, 
            int page, 
            LogLevel? logLevel = null,
            int pageSize = 30
        )
        {
            page = page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            using var session = Session;
            var logs = await session.GetNamedQuery("Log.getList")
                .SetParameter("applicationId", applicationId)
                .SetParameter("logLevel", logLevel)
                .SetFirstResult(offset)
                .SetMaxResults(pageSize)
                .ListAsync<LogEntity>();
            var count = await session.Query<LogEntity>()
                .Where(record => record.Application.Id == applicationId)
                .LongCountAsync();
            return (logs, count);
        }
        
        public async Task<bool> IsOwner(long userId, long logId)
        {
            using var session = Session;
            return await session.Query<LogEntity>()
                .Where(log => log.Application.User.Id == userId && log.Id == logId)
                .AnyAsync();
        }

        public async Task<bool> SetIsFavoriteAsync(long logId, bool isFavorite)
        {
            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                var log = await session.GetAsync<LogEntity>(logId);
                if (log == null)
                {
                    return false;
                }

                log.IsFavorite = isFavorite;
                await session.UpdateAsync(log);
                await transaction.CommitAsync();
                return true;
            }
        }
        
        public async Task<bool> IsLogExists(string token)
        {
            using var session = Session;
            return await session.Query<LogEntity>()
                .Where(log => log.Token == token)
                .AnyAsync();
        }
    }
}