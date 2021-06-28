using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka;

namespace OffLogs.WorkerService.LogProcessing
{
    public class LogProcessingService: ILogProcessingService
    {
        private readonly ILogger<LogProcessingService> _logger;
        private readonly IKafkaConsumerService _kafkaConsumerService;

        public LogProcessingService(ILogger<LogProcessingService> logger, IKafkaConsumerService kafkaConsumerService)
        {
            _logger = logger;
            _kafkaConsumerService = kafkaConsumerService;

            _logger.LogDebug("Init LogProcessingService...");
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Start execution...");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _kafkaConsumerService.ProcessLogsAsync(stoppingToken);
            }
            // Wait 1 sec before connect again
            Thread.Sleep(1000);
        }
    }
}