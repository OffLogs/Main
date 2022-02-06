using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.WorkerService.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.WorkerService.Services
{
    internal class NotificationProcessingHostedService : ABackgroundService
    {
        private readonly IKafkaNotificationsConsumerService _kafkaConsumerService;

        public NotificationProcessingHostedService(
            ILogger<ABackgroundService> logger,
            IKafkaNotificationsConsumerService kafkaConsumerService
        ) : base(logger)
        {
            _kafkaConsumerService = kafkaConsumerService;
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Notifications processing worker started at: {time}", DateTime.UtcNow);
            await _kafkaConsumerService.ProcessNotificationsAsync(cancellationToken);
        }
    }
}
