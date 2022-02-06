using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.WorkerService.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.WorkerService.Services
{
    internal class LogsProcessingHostedService : ABackgroundService
    {
        private readonly IKafkaLogsConsumerService _kafkaConsumerService;

        public LogsProcessingHostedService(
            ILogger<ABackgroundService> logger,
            IKafkaLogsConsumerService kafkaConsumerService
        ) : base(logger)
        {
            _kafkaConsumerService = kafkaConsumerService;
            ServiceName = "LogsProcessingHostedService";
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            LogDebug(string.Format("Logs processing worker started at: {0}", DateTime.Now));
            await _kafkaConsumerService.ProcessLogsAsync(cancellationToken);
        }
    }
}
