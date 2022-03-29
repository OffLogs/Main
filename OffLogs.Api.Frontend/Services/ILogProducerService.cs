using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Frontend.Services;

public interface ILogProducerService: IDomainService
{
    Task<LogEntity> AddToKafkaAsync(
        ApplicationEntity application,  
        string message,
        LogLevel level,
        DateTime timestamp,
        IDictionary<string, object> properties = null,
        ICollection<string> traces = null,
        string clientIp = null
    );
}
