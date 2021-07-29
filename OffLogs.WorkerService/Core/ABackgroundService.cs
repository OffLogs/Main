using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.WorkerService.Core
{
    internal abstract class ABackgroundService: BackgroundService
    {
        protected readonly ILogger<ABackgroundService> _logger;
        private CancellationToken _cancelationToken;
        private readonly CrontabSchedule _crontabScheduler;

        private DateTime _nextTickTime;
        private bool _isShouldRunWork
        {
            get => DateTime.UtcNow > _nextTickTime;
        }

        public ABackgroundService(ILogger<ABackgroundService> logger)
        {
            _logger = logger;
            _crontabScheduler = CrontabSchedule.Parse(
                GetCrontabExpression()
            );
            UpdateNextTickTime();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancelationToken = stoppingToken;
            _logger.LogInformation("Processing Hosted Service is starting.");

            stoppingToken.Register(() => _logger.LogDebug($"Processing Hosted Service is stopping because cancelled."));

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_isShouldRunWork)
                {
                    await DoWorkAsync(_cancelationToken);
                    UpdateNextTickTime();
                }
                Thread.Sleep(1000);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Processing Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        private void UpdateNextTickTime()
        {
            _nextTickTime = _crontabScheduler.GetNextOccurrence(DateTime.UtcNow, DateTime.MaxValue);
            _logger.LogDebug($"Processing Hosted Service. Next work scheduled at: {_nextTickTime}");
        }

        protected virtual string GetCrontabExpression() => "* * * * *";

        protected abstract Task DoWorkAsync(CancellationToken cancellationToken);
    }
}
