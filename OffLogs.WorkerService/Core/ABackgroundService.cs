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
        private readonly string _cronTabExpression = "* * * * *";

        protected readonly ILogger<ABackgroundService> _logger;
        private CancellationToken _cancelationToken;
        private readonly CrontabSchedule _crontabScheduler;

        private DateTime _nextTickTime;
        private bool _isShouldRunWork
        {
            get => DateTime.UtcNow > _nextTickTime;
        }

        public ABackgroundService(
            string cronTabExpression,
            ILogger<ABackgroundService> logger
        ) : this(logger) 
        {
            _cronTabExpression = cronTabExpression;
        }

        public ABackgroundService(ILogger<ABackgroundService> logger)
        {
            _logger = logger;
            _crontabScheduler = CrontabSchedule.Parse(_cronTabExpression);
            _nextTickTime = DateTime.UtcNow;
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

        protected abstract Task DoWorkAsync(CancellationToken cancellationToken);
    }
}
