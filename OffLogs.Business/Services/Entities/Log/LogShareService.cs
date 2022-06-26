using Commands.Abstractions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Exceptions.Common;

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
            if (!log.LogShares.Any(sh => sh.Log == log))
            {
                throw new ItemNotFoundException("Share not found");
            }
            log.LogShares.Clear();
            await _commandBuilder.SaveAsync(log);
        }

        public async Task<LogShareEntity> Share(LogEntity log)
        {
            if (log.LogShares.Any())
            {
                throw new ValidationException("Share already created for this log");
            }
            var logShare = new LogShareEntity();
            logShare.CreateTime = DateTime.UtcNow;
            logShare.UpdateTime = DateTime.UtcNow;
            logShare.Log = log;
            log.LogShares.Add(logShare);
            await _commandBuilder.SaveAsync(logShare);
            return logShare;
        }
    }
}
