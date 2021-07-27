using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Kafka.Models;

namespace OffLogs.Business.Services.Kafka
{
    public partial class KafkaConsumerService
    {
        public async Task<long> ProcessLogsAsync(CancellationToken cancellationToken)
        {
            return await ProcessLogsAsync(true, cancellationToken);
        }

        public async Task<long> ProcessLogsAsync(bool isInfiniteLoop = true, CancellationToken? cancellationToken = null)
        {
            CancellationTokenSource cancellationTokenSource;
            if (cancellationToken.HasValue)
            {
                cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken.Value);
            }
            else
            {
                cancellationTokenSource = new CancellationTokenSource();
            }
            var processedRecords = 0;
            using (var consumer = GetBuilder<LogMessageDto>().Build())
            {
                LogDebug($"Subscribe to {_logsTopicName}");
                consumer.Subscribe(_logsTopicName);

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
                                cancellationTokenSource.Cancel();
                                break;
                            }
                        }
                    }, cancellationTokenSource.Token);    
                }
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationTokenSource.Token);
                        if (consumeResult != null)
                        {
                            if (consumeResult.Message.Value != null)
                            {
                                await ProcessLogAsync(consumeResult.Message.Value);
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
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message, e);
                    }
                }
            }
            return await Task.FromResult(processedRecords);
        }

        private async Task ProcessLogAsync(LogMessageDto messageModel)
        {
            try
            {
                // 1. Validate JWT token
                var applicationId = _jwtApplicationService.GetApplicationId(messageModel.ApplicationJwtToken);
                if (!applicationId.HasValue)
                {
                    await LogMessageModel(messageModel, "Found log model with error!");
                    return;
                }
                var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(
                    applicationId.Value
                );
                if (application == null)
                {
                    await LogMessageModel(messageModel, "Application not found for the log message model!");
                    return;
                }
                
                // 2. Save log
                var entity = messageModel.GetEntity();
                var isExists = await _queryBuilder.For<bool>()
                    .WithAsync(new LogIsExistsByTokenCriteria(entity.Token));
                if (isExists)
                {
                    return;
                }
                entity.Application = application;
                await _commandBuilder.SaveAsync(entity);
                await _dbSessionProvider.PerformCommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
            await Task.CompletedTask;
        }

        private async Task LogMessageModel(LogMessageDto messageModel, string logMessage)
        {
            var request = new RequestLogEntity(
                RequestLogType.Log,
                messageModel.ClientIp,
                messageModel,
                messageModel.ApplicationJwtToken    
            );
            await _commandBuilder.SaveAsync(request);
            _logger.LogError(logMessage);
        }
    }
}