using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Orm.Commands.Entities.Log;
using Persistence.Transactions.Behaviors;

namespace OffLogs.WorkerService.LogDeletion
{
    public class LogDeletionService : ILogDeletionService
    {
        private TimeSpan _timeout = TimeSpan.FromHours(24);

        private readonly ILogger<LogDeletionService> _logger;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IDbSessionProvider _sessionProvider;

        public LogDeletionService(
            ILogger<LogDeletionService> logger, 
            IAsyncCommandBuilder commandBuilder,
            IDbSessionProvider sessionProvider
        )
        {
            _logger = logger;
            _commandBuilder = commandBuilder;
            _sessionProvider = sessionProvider;
            _logger.LogDebug("Init LogProcessingService...");
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Log deletion worker running at: {time}", DateTimeOffset.Now);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _commandBuilder.ExecuteAsync(
                        new LogDeleteSpoiledCommandContext(),
                        stoppingToken
                    );
                    await _sessionProvider.PerformCommitAsync(stoppingToken);

                    _logger.LogInformation("Log deletion work is completed. Waiting for next time...");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }

                // Wait 1 sec before connect again
                Thread.Sleep(_timeout);
            }
        }
    }
}