using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Commands.Entities.Log;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.Log
{
    public class LogService: ILogService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly ILogAssembler _logAssembler;

        public LogService(
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder queryBuilder,
            ILogAssembler logAssembler
        )
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
            _logAssembler = logAssembler;
        }
        
        public async Task<LogEntity> AddAsync(LogEntity log)
        {
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
            IDictionary<string, object> properties = null,
            ICollection<string> traces = null
        )
        {
            var log = await _logAssembler.AssembleEncryptedLogAsync(
                application,
                message,
                level,
                timestamp,
                properties,
                traces
            );
            await _commandBuilder.SaveAsync(log);
            return log;
        }

        public async Task<LogEntity> GetOneAsync(long logId, byte[] privateKey)
        {
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(logId);
            if (log == null)
            {
                throw new ItemNotFoundException(nameof(log));
            }

            return await _logAssembler.AssembleDecryptedLogAsync(log, privateKey);
        }
    }
}
