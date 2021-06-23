using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Kafka.Deserializers;
using OffLogs.Business.Services.Kafka.Models;

namespace OffLogs.Business.Services.Kafka
{
    public partial class KafkaConsumerService: IKafkaConsumerService
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

        private ConsumerBuilder<string, T> GetBuilder<T>()
        {
            var builder = new ConsumerBuilder<string, T>(_config);
            builder.SetValueDeserializer(new JsonValueDeserializer<T>(_logger));
            builder.SetKeyDeserializer(new KeyDeserializer());
            builder.SetErrorHandler((producer, error) =>
            {
                _logger.LogError($"Kafka consumer error: Code: {error.Code}, IsBroker: {error.IsBrokerError}, Reason: {error.Reason}");
            });
            return builder;
        }
    }
}