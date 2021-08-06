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

namespace OffLogs.WorkerService.Services
{
    internal class LogsDeletionHostedService : ABackgroundService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IDbSessionProvider _sessionProvider;
        private readonly IAsyncNotificationBuilder _notificationBuilder;
        private readonly IConfigurationService _configurationService;

        public LogsDeletionHostedService(
            ILogger<ABackgroundService> logger,
            IAsyncCommandBuilder commandBuilder,
            IDbSessionProvider sessionProvider,
            IAsyncNotificationBuilder notificationBuilder,
            IConfigurationService configurationService
        ) : base(
            logger
        )
        {
            _commandBuilder = commandBuilder;
            _sessionProvider = sessionProvider;
            _notificationBuilder = notificationBuilder;
            _configurationService = configurationService;
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
                _logger.LogDebug("Log deletion work is started.");

                await _commandBuilder.ExecuteAsync(
                    new LogDeleteSpoiledCommandContext(),
                    cancellationToken
                );
                await _sessionProvider.PerformCommitAsync(cancellationToken);
                await _notificationBuilder.SendAsync(new LogsDeletedNotificationContext(
                    _configurationService.SupportEmail
                ));
                
                _logger.LogInformation("Log deletion work is completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
