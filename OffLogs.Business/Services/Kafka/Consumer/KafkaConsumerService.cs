using System;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka.Deserializers;
using System.Timers;
using Timer = System.Timers.Timer;
using Commands.Abstractions;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using OffLogs.Business.Helpers;
using OffLogs.Business.Services.Kafka.Models;

namespace OffLogs.Business.Services.Kafka.Consumer
{
    public abstract class KafkaConsumerService<TKafkaDto> : IDisposable
        where TKafkaDto : IKafkaDto
    {
        private readonly TimeSpan _defaultWaitTimeout = TimeSpan.FromSeconds(10);

        protected readonly IConfiguration _configuration;
        protected readonly ConsumerConfig _config;
        private readonly Timer _timerProcessedCounter;
        private long _processedLogsCounter;

        protected string ConsumerName;
        protected readonly string _groupName;
        protected readonly string _clientId;
        protected readonly string _kafkaServers;
        protected readonly ILogger<IKafkaProducerService> _logger;

        public KafkaConsumerService(
            IConfiguration configuration,
            ILogger<IKafkaProducerService> logger
        )
        {
            _configuration = configuration;
            _logger = logger;
            var kafkaSection = configuration.GetSection("Kafka");
            _groupName = kafkaSection.GetValue<string>("ConsumerGroup");
            _clientId = kafkaSection.GetValue<string>("ConsumerClientId") + "-" + EnvironmentHelper.GetPodId();
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

        public void Dispose()
        {
            _timerProcessedCounter.Stop();
        }

        protected ConsumerBuilder<string, T> GetBuilder<T>()
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

        protected void LogDebug(string message)
        {
            _logger.LogDebug($"Kafka {ConsumerName} Consumer: {message}");
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

        public async Task<long> ProcessMessagesAsync(
            string topicName,
            bool isInfiniteLoop = true,
            CancellationToken? cancellationToken = null
        )
        {
            CancellationTokenSource timeoutCancellationToken;
            if (cancellationToken.HasValue)
            {
                timeoutCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken.Value);
            }
            else
            {
                timeoutCancellationToken = new CancellationTokenSource();
            }
            var processedRecords = 0;
            using (var consumer = GetBuilder<TKafkaDto>().Build())
            {
                LogDebug($"Subscribe to {topicName}");
                consumer.Subscribe(topicName);

                var startTime = DateTime.UtcNow;
                if (!isInfiniteLoop)
                {
                    var task = Task.Run(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(100);
                            var difference = DateTime.UtcNow - startTime;
                            if (difference >= _defaultWaitTimeout)
                            {
                                timeoutCancellationToken.Cancel();
                                break;
                            }
                        }
                    }, timeoutCancellationToken.Token);
                }
                while (!timeoutCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(timeoutCancellationToken.Token);
                        if (consumeResult != null)
                        {
                            if (consumeResult.Message.Value != null)
                            {
                                var startProcessingTime = DateTime.UtcNow;
                                await ProcessItemAsync(consumeResult.Message.Value);
                                var processingTime = DateTime.UtcNow - startProcessingTime;
                                _logger.LogDebug(
                                    $"Kafka message processing time: {processingTime.TotalMilliseconds} ms"
                                );
                            }

                            // Increase global counter
                            IncreaseProcessedMessagesCounter();
                            consumer.StoreOffset(consumeResult);
                            consumer.Commit();

                            // Increase local counter
                            processedRecords++;
                        }
                    }
                    catch (OperationCanceledException e)
                    {
                        LogDebug($"Operation was cancelled via cancellation token");
                        consumer.Close();
                        // Cancellation token was canceled
                        break;
                    }
                    catch (KafkaException e)
                    {
                        _logger.LogError($"KafkaException. ${e.Message}", e);
                        timeoutCancellationToken.Cancel();
                        break;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message, e);
                    }
                }
            }
            return await Task.FromResult(processedRecords);
        }

        private void IncreaseProcessedMessagesCounter()
        {
            _processedLogsCounter++;
        }

        protected abstract Task ProcessItemAsync(TKafkaDto dto);
    }
}
