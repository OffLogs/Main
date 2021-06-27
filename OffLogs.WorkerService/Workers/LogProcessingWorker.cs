using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka;

namespace OffLogs.WorkerService.Workers
{
    public class LogProcessingWorker : BackgroundService
    {
        private readonly ILogger<LogProcessingWorker> _logger;
        private readonly IKafkaConsumerService _kafkaConsumerService;

        public LogProcessingWorker(ILogger<LogProcessingWorker> logger, IKafkaConsumerService kafkaConsumerService)
        {
            _logger = logger;
            _kafkaConsumerService = kafkaConsumerService;

            _logger.LogDebug("Init LogProcessingWorker...");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Start execution...");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _kafkaConsumerService.ProcessLogsAsync(stoppingToken);
            }
        }
    }
}