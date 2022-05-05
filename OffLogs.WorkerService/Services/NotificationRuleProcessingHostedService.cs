using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.WorkerService.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Notifications;

namespace OffLogs.WorkerService.Services
{
    internal class NotificationRuleProcessingHostedService : ABackgroundService
    {
        private readonly INotificationRuleProcessingService _processingService;

        public NotificationRuleProcessingHostedService(
            ILogger<ABackgroundService> logger,
            INotificationRuleProcessingService processingService
        ) : base(logger)
        {
            _processingService = processingService;
            ServiceName = "NotificationRuleProcessingHostedService";
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            LogDebug($"Notification rules processing worker started at: {DateTime.UtcNow}");
            await _processingService.FindAndProcessWaitingRules(cancellationToken);
        }
    }
}
