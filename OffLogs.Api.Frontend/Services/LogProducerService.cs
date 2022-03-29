using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Kafka;

namespace OffLogs.Api.Frontend.Services;

public class LogProducerService: ILogProducerService
{
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly ILogAssembler _logAssembler;

    public LogProducerService(
        IKafkaProducerService kafkaProducerService,
        ILogAssembler logAssembler
    )
    {
        _kafkaProducerService = kafkaProducerService;
        _logAssembler = logAssembler;
    }

    public async Task<LogEntity> AddToKafkaAsync(
        ApplicationEntity application,  
        string message,
        LogLevel level,
        DateTime timestamp,
        IDictionary<string, object> properties = null,
        ICollection<string> traces = null,
        string clientIp = null
    )
    {
        var log = await _logAssembler.AssembleLog(
            application,
            message,
            level,
            timestamp,
            properties,
            traces
        );
        await _kafkaProducerService.ProduceLogMessageAsync(log, clientIp);
        return log;
    }
}
