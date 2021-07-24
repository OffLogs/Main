using Commands.Abstractions;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Entities.Log
{
    public class LogShareService : ILogShareService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;

        public LogShareService(IAsyncCommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
        }

        public async Task DeleteShare(LogEntity log)
        {
            if (log.LogShare != null)
            {
                throw new ItemNotFoundException("Share not found");
            }
            log.LogShare = null;
            await _commandBuilder.SaveAsync(log);
        }

        public async Task<LogShareEntity> Share(LogEntity log)
        {
            if (log.LogShare != null)
            {
                throw new ValidationException("Share already created for this log");
            }
            var logShare = new LogShareEntity();
            logShare.Log = log;
            log.LogShare = logShare;
            await _commandBuilder.SaveAsync(logShare);
            return logShare;
        }
    }
}
