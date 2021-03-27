using System;
using System.Collections.Generic;
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
            using (var aaa = Connection.SqlProc("pr_log_add", parameters))
            {
                var bb = 123;
            }

            ;
            // await ExecuteWithReturnAsync("pr_log_add", parameters);
        }
        
        public async Task<(IEnumerable<LogEntity>, int)> GetList(long applicationId, int page, int pageSize = 30)
        {
            var sumCounter = 0;
            var result = new List<LogEntity>();
            page = page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            var countQuery = Connection.From<LogEntity>()
                .Where(log => log.ApplicationId == applicationId)
                .ToCountStatement();
            var listQuery = Connection.From<LogEntity>()
                .LeftJoin<LogEntity, LogPropertyEntity>((log, property) => log.Id == property.LogId)
                .LeftJoin<LogEntity, LogTraceEntity>((log, trace) => log.Id == trace.LogId)
                .Where<LogEntity>(log => log.ApplicationId == applicationId)
                .Limit(offset, pageSize)
                .OrderBy<LogEntity>(log => log.CreateTime)
                .Select<LogEntity, LogPropertyEntity, LogTraceEntity>((log, property, trace) => new
                {
                    log,
                    property,
                    trace,
                    sumCount = Sql.Custom($"({countQuery})")
                });
            var selectResult = Connection.SelectAsync<LogEntity>(listQuery);

            var query = await Connection.QueryAsync<LogEntity, LogPropertyEntity, LogTraceEntity, int, LogEntity>(
                sql: listQuery.ToSelectStatement(),
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
                splitOn: "Id,Id,Id,sumCount"
            );
            return (result, sumCounter);
        }
    }
}