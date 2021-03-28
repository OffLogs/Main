using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Extensions;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using ServiceStack.Text;
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
        
        public async Task<LogEntity> AddAsync(
            long applicationId,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        )
        {
            var log = new LogEntity()
            {
                ApplicationId = applicationId,
                Message = message,
                Level = level,
                LogTime = timestamp,
                Properties = properties?.ToList(),
                Traces = traces?.ToList(),
            };
            await Connection.SaveAsync(log, true);
            return log;
        }
        
        public async Task<(IEnumerable<LogEntity>, long)> GetList(long applicationId, int page, int pageSize = 30)
        {
            var result = new List<LogEntity>();
            page = page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            var sumCounter = await Connection.CountAsync<LogEntity>(log => log.ApplicationId == applicationId);
            
            var logIdsQuery = Connection.From<LogEntity>()
                .Where<LogEntity>(log => log.ApplicationId == applicationId)
                .Limit(offset, pageSize)
                .Select(log => log.Id);
            var listQuery = Connection.From<LogEntity>()
                .LeftJoin<LogEntity, LogPropertyEntity>((log, property) => log.Id == property.LogId)
                .LeftJoin<LogEntity, LogTraceEntity>((log, trace) => log.Id == trace.LogId)
                .Where(log => Sql.In(log.Id, logIdsQuery))
                .OrderBy<LogEntity>(log => log.CreateTime)
                .Select("*");
            var selectResult = await Connection.SelectMultiAsync<LogEntity, LogPropertyEntity, LogTraceEntity>(listQuery);
            foreach (var (log, property, trace) in selectResult)
            {
                var existsLog = result.FirstOrDefault(innerLog => innerLog.Id == log.Id);
                if (existsLog == null)
                {
                    existsLog = log;
                    result.Add(existsLog);
                }
                if (property != null && !existsLog.Properties.Exists(innerProp => innerProp.Id == property?.Id))
                {
                    existsLog.Properties.Add(property);    
                }
                if (trace != null && !existsLog.Traces.Exists(innerTrace => innerTrace.Id == trace?.Id))
                {
                    existsLog.Traces.Add(trace);    
                }
            }
            return (result, sumCounter);
        }
    }
}