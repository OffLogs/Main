using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using OffLogs.Business.Services.Jwt;
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
    }
}