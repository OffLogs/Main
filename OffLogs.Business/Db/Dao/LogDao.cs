using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Multi;
using OffLogs.Business.Db.Entity;
using LogLevel = OffLogs.Business.Constants.LogLevel;

namespace OffLogs.Business.Db.Dao
{
    public class LogDao: CommonDao, ILogDao
    {
        public LogDao(
            IConfiguration configuration, 
            ILogger<LogDao> logger
        ) : base(
            configuration,
            logger
        )
        {
        }

        public async Task<LogEntity> GetLogAsync(long logId)
        {
            using (var session = Session)
            {
                var logQuery = session.Query<LogEntity>()
                    .Where(record => record.Id == logId)
                    .Fetch(record => record.Application)
                    .FetchMany(record => record.Properties);
                var tracesQuery = session.Query<LogTraceEntity>()
                    .Where(record => record.Log.Id == logId);

                var queries = session.CreateQueryBatch()
                    .Add("log", logQuery)
                    .Add("traces", tracesQuery);

                var log = queries.GetResult<LogEntity>("log").SingleOrDefault();
                if (log != null)
                {
                    log.Traces = queries.GetResult<LogTraceEntity>("traces").ToList();
                }
                return log;    
            }
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
            int pageSize = 30
        )
        {
            page = page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            using var session = Session;
            var list = await session.GetNamedQuery("getLogWithData")
                .SetInt64("applicationId", applicationId)
                .SetFirstResult(offset)
                .SetMaxResults(pageSize)
                .ListAsync<LogEntity>();
            var countQuery = await session.Query<LogEntity>()
                .Where(record => record.Application.Id == applicationId)
                .CountAsync();
            return (list, countQuery);
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
    }
}