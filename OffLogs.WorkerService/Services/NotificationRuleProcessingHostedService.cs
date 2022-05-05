using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.WorkerService.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Services.Entities.NotificationRule;

namespace OffLogs.WorkerService.Services
{
    internal class NotificationRuleProcessingHostedService : ABackgroundService
    {
        private readonly INotificationRuleService _ruleService;

        public NotificationRuleProcessingHostedService(
            ILogger<ABackgroundService> logger,
            INotificationRuleService ruleService
        ) : base(logger)
        {
            _ruleService = ruleService;
            ServiceName = "NotificationRuleProcessingHostedService";
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            LogDebug($"Notification rules processing worker started at: {DateTime.UtcNow}");
            await _ruleService.GetNextAndSetExecutingAsync(cancellationToken);
        }
    }
}
