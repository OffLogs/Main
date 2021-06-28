using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;
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
            using (var consumer = GetBuilder<LogMessageModel>().Build())
            {
                LogDebug($"Subscribe to {_logsTopicName}");
                consumer.Subscribe(_logsTopicName);

                var startTime = DateTime.Now;
                if (!isInfiniteLoop)
                {
                    var task = Task.Run(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(100);
                            var difference = DateTime.Now - startTime;
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

        private async Task ProcessLogAsync(LogMessageModel messageModel)
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
                ApplicationEntity application = await _applicationDao.GetAsync(applicationId.Value);
                if (application == null)
                {
                    await LogMessageModel(messageModel, "Application not found for the log message model!");
                    return;
                }
                
                // 2. Save log
                var entity = messageModel.GetEntity();
                var isExists = await _logDao.IsLogExists(entity.Token);
                if (isExists)
                {
                    return;
                }
                entity.Application = application;
                await _logDao.AddAsync(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
            await Task.CompletedTask;
        }

        private async Task LogMessageModel(LogMessageModel messageModel, string logMessage)
        {
            await _requestLogDao.AddAsync(
                RequestLogType.Log,
                messageModel.ClientIp,
                messageModel,
                messageModel.ApplicationJwtToken
            );
            _logger.LogError(logMessage);
        }
    }
}