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
    public partial class KafkaConsumerService
    {
        public async Task<long> ProcessLogsAsync(bool isInfiniteLoop = true, CancellationToken cancellationToken = default)
        {
            var processedRecords = 0;
            using (var consumer = GetBuilder<LogMessageModel>().Build())
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
                        if (consumeResult.Message.Value != null)
                        {
                            await ProcessLogAsync(consumeResult.Message.Value);
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

                    if (consumeResult.Message.Value != null)
                    {
                        await ProcessLogAsync(consumeResult.Message.Value);
                    }
                }
            }
            return await Task.FromResult(processedRecords);
        }

        async Task ProcessLogAsync(LogMessageModel innerConsumer)
        {
            await Task.CompletedTask;
        }
    }
}