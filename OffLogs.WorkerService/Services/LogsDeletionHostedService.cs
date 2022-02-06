using Commands.Abstractions;
using Microsoft.Extensions.Logging;
using Notification.Abstractions;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Orm.Commands.Entities.Log;
using OffLogs.Business.Services;
using OffLogs.WorkerService.Core;
using Persistence.Transactions.Behaviors;
using System;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Services.Kafka;

namespace OffLogs.WorkerService.Services
{
    internal class LogsDeletionHostedService : ABackgroundService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IDbSessionProvider _sessionProvider;
        private readonly IKafkaProducerService _producerService;
        private readonly IConfigurationService _configurationService;

        public LogsDeletionHostedService(
            ILogger<ABackgroundService> logger,
            IAsyncCommandBuilder commandBuilder,
            IDbSessionProvider sessionProvider,
            IKafkaProducerService producerService,
            IConfigurationService configurationService
        ) : base(
            logger
        )
        {
            _commandBuilder = commandBuilder;
            _sessionProvider = sessionProvider;
            _configurationService = configurationService;
            _producerService = producerService;
            ServiceName = "LogsDeletionHostedService";
        }

        protected override string GetCrontabExpression()
        {
            // At 00:00.
            return "0 0 * * *";
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            try
            {
                LogDebug(string.Format("Logs processing worker started at: {0}", DateTime.Now));

                await _commandBuilder.ExecuteAsync(
                    new LogDeleteSpoiledCommandContext(),
                    cancellationToken
                );
                await _sessionProvider.PerformCommitAsync(cancellationToken);
                await _producerService.ProduceNotificationMessageAsync(new LogsDeletedNotificationContext(
                    _configurationService.SupportEmail
                ));                
                
                LogDebug("Log deletion work is completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
