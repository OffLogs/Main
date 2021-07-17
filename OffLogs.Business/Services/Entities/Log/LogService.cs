using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Business.Services.Entities.Log
{
    public class LogService: ILogService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IJwtApplicationService _jwtService;

        public LogService(
            IAsyncCommandBuilder commandBuilder, 
            IJwtApplicationService jwtService
        )
        {
            _commandBuilder = commandBuilder;
            _jwtService = jwtService;
        }
        
        public async Task<LogEntity> AddAsync(LogEntity log)
        {
            // Clear bags
            var properties = log.Properties.ToList();
            log.Properties = new List<LogPropertyEntity>();
            var traces = log.Traces.ToList();
            log.Traces = new List<LogTraceEntity>();
                
            await _commandBuilder.SaveAsync(log);

            properties.ForEach(log.AddProperty);
            traces.ForEach(log.AddTrace);
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
            await _commandBuilder.SaveAsync(log);
            return log;
        }
    }
}