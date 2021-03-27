using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Extensions;
using ServiceStack.OrmLite.Dapper;
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

        public async Task AddAsync(
            long applicationId,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        )
        {
            var parameters = new DynamicParameters(new
            {
                ApplicationId = applicationId,
                Message = message,
                Level = level.GetValue(),
                Timestamp = timestamp
            });
            parameters.AddTable("@Properties", "[dbo].[LogPropertyType]", properties ?? new List<LogPropertyEntity>());
            parameters.AddTable("@Traces", "[dbo].[LogTraceType]", traces ?? new List<LogTraceEntity>());
            await ExecuteWithReturnAsync("pr_LogAdd", parameters);
        }
        
        public async Task<(IEnumerable<LogEntity>, int)> GetList(long applicationId, int page)
        {
            var sumCounter = 0;
            var result = new List<LogEntity>();
            var parameters = new
            {
                ApplicationId = applicationId,
                Page = page,
                PageSize = 30,
                Offset = 3
            };
            var query = await Connection.QueryAsync<LogEntity, LogPropertyEntity, LogTraceEntity, int, LogEntity>(
                sql: GetQuery("log_get_list"),
                map: (log, property, trace, count) =>
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
                    sumCounter = count;
                    return log;
                },
                param: parameters,
                splitOn: "Id,Id,Id,sumCount"
            );
            return (result, sumCounter);
        }
    }
}