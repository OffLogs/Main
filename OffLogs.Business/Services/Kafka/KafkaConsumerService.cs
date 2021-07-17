using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka.Deserializers;
using System.Timers;
using OffLogs.Business.Dao;
using Timer = System.Timers.Timer;
using Commands.Abstractions;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Kafka
{
    public partial class KafkaConsumerService: IKafkaConsumerService
    {
        private readonly TimeSpan _defaultWaitTimeout = TimeSpan.FromSeconds(5);
        
        private readonly IConfiguration _configuration;
        private readonly string _groupName;
        private readonly string _clientId;
        private readonly string _kafkaServers;
        private readonly ILogger<IKafkaProducerService> _logger;
        private readonly IJwtApplicationService _jwtApplicationService;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly ConsumerConfig _config;
        private readonly string _logsTopicName;
        private readonly Timer _timerProcessedCounter;
        private long _processedLogsCounter;

        public KafkaConsumerService(
            IConfiguration configuration, 
            ILogger<IKafkaProducerService> logger,
            IJwtApplicationService jwtApplicationService,
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder queryBuilder
        )
        {
            _configuration = configuration;
            _logger = logger;
            _jwtApplicationService = jwtApplicationService;
            this._commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
            var kafkaSection = configuration.GetSection("Kafka");
            _groupName = kafkaSection.GetValue<string>("ConsumerGroup");
            _clientId = kafkaSection.GetValue<string>("ConsumerClientId");
            _logsTopicName = kafkaSection.GetValue<string>("Topic:Logs");
            _kafkaServers = kafkaSection.GetValue<string>("Servers");
            
            _config = new ConsumerConfig
            {
                BootstrapServers = _kafkaServers,
                GroupId = _groupName,
                ClientId = _clientId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false, // (the default)
                EnableAutoOffsetStore = false,
                AllowAutoCreateTopics = false,
            };

            _timerProcessedCounter = new Timer(5000);
            _timerProcessedCounter.Elapsed += OnProcessedCounterTimerTick;
            _timerProcessedCounter.Start();
        }

        ~KafkaConsumerService()
        {
            _timerProcessedCounter.Stop();
        }

        private ConsumerBuilder<string, T> GetBuilder<T>()
        {
            LogDebug($"Build new instance: {_kafkaServers}");
            var builder = new ConsumerBuilder<string, T>(_config);
            builder.SetValueDeserializer(new JsonValueDeserializer<T>(_logger));
            builder.SetKeyDeserializer(new KeyDeserializer());
            builder.SetErrorHandler((producer, error) =>
            {
                _logger.LogError($"Kafka consumer error: Code: {error.Code}, IsBroker: {error.IsBrokerError}, Reason: {error.Reason}");
            });
            return builder;
        }

        private void LogDebug(string message)
        {
            _logger.LogDebug($"Kafka Consumer: {message}");
        }
        
        private void OnProcessedCounterTimerTick(object sender, ElapsedEventArgs e)
        {
            if (_processedLogsCounter > 0)
            {
                // We don't want wait until log message will be written
                var counter = _processedLogsCounter;
                _processedLogsCounter = 0;
                LogDebug($"Processed messages counter: {counter}/{_timerProcessedCounter.Interval / 1000} sec");
            }
        }
        
        private void IncreaseProcessedMessagesCounter()
        {
            _processedLogsCounter++;
        }
    }
}