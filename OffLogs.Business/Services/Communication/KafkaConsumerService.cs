using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Communication.Deserializers;
using OffLogs.Business.Services.Communication.Serializers;

namespace OffLogs.Business.Services.Communication
{
    public class KafkaConsumerService: IKafkaConsumerService
    {
        private readonly int _defaultWaitTimeout = 5000;
        
        private readonly IConfiguration _configuration;
        private readonly string _groupName;
        private readonly ILogger<IKafkaProducerService> _logger;
        private readonly ILogDao _logDao;
        private readonly ConsumerConfig _config;
        private readonly string _logsTopicName;

        public KafkaConsumerService(
            IConfiguration configuration, 
            ILogger<IKafkaProducerService> logger,
            ILogDao logDao
        )
        {
            _configuration = configuration;
            _logger = logger;
            _logDao = logDao;

            var kafkaSection = configuration.GetSection("Kafka");
            _groupName = kafkaSection.GetValue<string>("ConsumerGroup");
            _logsTopicName = kafkaSection.GetValue<string>("Topic:Logs");
            var kafkaServers = kafkaSection.GetValue<string>("Servers");
            
            _config = new ConsumerConfig
            {
                BootstrapServers = kafkaServers,
                GroupId = _groupName,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true, // (the default)
                EnableAutoOffsetStore = false,
                AutoCommitIntervalMs = 5000
            };
        }

        public async Task<long> ProcessLogsAsync(bool isInfiniteLoop = true, CancellationToken cancellationToken = default)
        {
            var processedRecords = 0;
            using (var consumer = GetBuilder<LogEntity>().Build())
            {
                consumer.Subscribe(_logsTopicName);

                if (isInfiniteLoop)
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult != null)
                        {
                            consumer.StoreOffset(consumeResult);
                            processedRecords++;    
                        }
                    }
                }
                else
                {
                    var consumeResult = consumer.Consume(_defaultWaitTimeout);
                    if (consumeResult != null)
                    {
                        consumer.StoreOffset(consumeResult);
                        processedRecords++;    
                    }
                }
            }

            async Task ConsumeAsync(IConsumer<Null, LogEntity> innerConsumer)
            {
                var consumeResult = innerConsumer.Consume(cancellationToken);

                try
                {
                    var log = consumeResult.Message.Value;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }

                if (consumeResult != null)
                {
                    innerConsumer.StoreOffset(consumeResult);
                    processedRecords++;    
                }

                await Task.CompletedTask;
            }
            
            return await Task.FromResult(processedRecords);
        }

        private ConsumerBuilder<Null, T> GetBuilder<T>()
        {
            var builder = new ConsumerBuilder<Null, T>(_config);
            builder.SetValueDeserializer(new JsonDeserializer<T>());
            builder.SetErrorHandler((producer, error) =>
            {
                _logger.LogError($"Kafka consumer error: Code: {error.Code}, IsBroker: {error.IsBrokerError}, Reason: {error.Reason}");
            });
            return builder;
        }
    }
}