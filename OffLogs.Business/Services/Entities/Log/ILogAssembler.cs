using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.Log;

public interface ILogAssembler: IDomainService
{
    public Task<LogEntity> AssembleLog(
        ApplicationEntity application,
        string message,
        LogLevel level,
        DateTime timestamp,
        IDictionary<string, object> properties = null,
        ICollection<string> traces = null
    );
}
