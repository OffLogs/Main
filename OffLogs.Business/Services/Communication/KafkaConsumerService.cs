using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Communication.Deserializers;
using OffLogs.Business.Services.Communication.Serializers;

namespace OffLogs.Business.Services.Communication
{
    public class KafkaConsumerService: IKafkaConsumerService
    {
        private readonly IConfiguration _configuration;
        private readonly string _groupName;
        private readonly ILogger<IKafkaProducerService> _logger;
        private readonly ConsumerConfig _config;
        private readonly string _logsTopicName;

        public KafkaConsumerService(IConfiguration configuration, ILogger<IKafkaProducerService> logger)
        {
            _configuration = configuration;
            _logger = logger;

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
                EnableAutoOffsetStore = false
            };
        }

        public async Task<long> ProcessLogsAsync(CancellationToken cancellationToken = default)
        {
            var processedRecords = 0;
            var commitPeriod = 10;
            using (var consumer = GetBuilder<LogEntity>().Build())
            {
                consumer.Subscribe(_logsTopicName); 
                
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    var aaa = 123;

                    consumer.StoreOffset(consumeResult);
                    processedRecords++;
                }
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