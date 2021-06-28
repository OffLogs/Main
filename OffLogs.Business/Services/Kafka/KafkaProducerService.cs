using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Kafka.Models;
using OffLogs.Business.Services.Kafka.Serializers;

namespace OffLogs.Business.Services.Kafka
{
    public class KafkaProducerProducerService: IKafkaProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IKafkaProducerService> _logger;
        private readonly ProducerConfig _producerConfig;
        private readonly string _producerId;
        private readonly string _kafkaServers;
        private readonly string _logsTopicName;
        
        private IProducer<string, object> _producer;
        private IProducer<string, object> Producer
        {
            get
            {
                if (_producer == null)
                {
                    LogDebug("Build new instance");
                    
                    var builder = new ProducerBuilder<string, object>(_producerConfig);
                    builder.SetValueSerializer(new ValueSerializer<object>());
                    builder.SetKeySerializer(new ValueSerializer<string>());
                    builder.SetErrorHandler((producer, error) =>
                    {
                        _logger.LogError($"Kafka producer error: Code: {error.Code}, IsBroker: {error.IsBrokerError}, Reason: {error.Reason}");
                    });
                    _producer = builder.Build();
                }

                return _producer;
            }
        }

        public KafkaProducerProducerService(IConfiguration configuration, ILogger<IKafkaProducerService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var kafkaSection = configuration.GetSection("Kafka");
            _producerId = kafkaSection.GetValue<string>("ProducerId");
            _logsTopicName = kafkaSection.GetValue<string>("Topic:Logs");
            _kafkaServers = kafkaSection.GetValue<string>("Servers");
            
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaServers,
                ClientId = _producerId, 
                Acks = Acks.All,
                MessageSendMaxRetries = 2000,
                SecurityProtocol = SecurityProtocol.Plaintext
            };
            
            LogDebug($"Init service: {_kafkaServers}");
        }

        ~KafkaProducerProducerService()
        {
            _producer?.Dispose();
            _producer = null;
        }

        public async Task ProduceLogMessageAsync(string applicationJwt, LogEntity logEntity, string clientIp = null)
        {
            var modelToSend = new LogMessageModel(applicationJwt, logEntity)
            {
                ClientIp = clientIp
            };
            await Producer.ProduceAsync(_logsTopicName, new Message<string, object>
            {
                Key = modelToSend.Token,
                Value = modelToSend
            });
        }
        
        public void Flush(CancellationToken cancellationToken = default)
        {
            Producer.Flush(cancellationToken);
        }
        
        private void LogDebug(string message)
        {
            _logger.LogDebug($"Kafka Producer: {message}");
        }
    }
}