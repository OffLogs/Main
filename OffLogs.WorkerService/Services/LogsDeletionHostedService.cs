using Commands.Abstractions;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Orm.Commands.Entities.Log;
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

        public LogsDeletionHostedService(
            ILogger<ABackgroundService> logger,
            IAsyncCommandBuilder commandBuilder,
            IDbSessionProvider sessionProvider
        ) : base(
            logger
        )
        {
            _commandBuilder = commandBuilder;
            _sessionProvider = sessionProvider;
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

                _logger.LogInformation("Log deletion work is completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
