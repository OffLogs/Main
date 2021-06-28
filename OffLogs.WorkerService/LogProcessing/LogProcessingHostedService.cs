using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OffLogs.WorkerService.LogProcessing
{
    public class LogProcessingHostedService: BackgroundService
    {
        private readonly ILogger<LogProcessingHostedService> _logger;
        private Timer _timer;
        
        public IServiceProvider Services { get; }
        
        public LogProcessingHostedService(
            IServiceProvider services,
            ILogger<LogProcessingHostedService> logger
        )
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Log Processing Hosted Service is starting.");
            
            stoppingToken.Register(() => _logger.LogDebug($"Log Processing Hosted Service is stopping because cancelled."));
            
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService = 
                    scope.ServiceProvider
                        .GetRequiredService<ILogProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }
        
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Log Processing Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            await base.StopAsync(stoppingToken);
        }
    }
}