using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Commands.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka.Models;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Kafka.Consumer
{
    public partial class KafkaLogsConsumerService : KafkaConsumerService<LogMessageDto>, IKafkaLogsConsumerService
    {
        private readonly IJwtApplicationService _jwtApplicationService;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IDbSessionProvider _dbSessionProvider;
        private readonly string _logsTopicName;

        public KafkaLogsConsumerService(
            IConfiguration configuration,
            ILogger<IKafkaProducerService> logger,
            IJwtApplicationService jwtApplicationService,
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder queryBuilder,
            IDbSessionProvider dbSessionProvider
        ) : base(
            configuration,
            logger
        )
        {
            ConsumerName = "LogsConsumer";
            _jwtApplicationService = jwtApplicationService;
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
            _dbSessionProvider = dbSessionProvider;
            _logsTopicName = configuration.GetValue<string>("Kafka:Topic:Logs");
        }

        public async Task<long> ProcessLogsAsync(CancellationToken cancellationToken)
        {
            return await ProcessMessagesAsync(_logsTopicName, true, cancellationToken);
        }

        public async Task<long> ProcessLogsAsync(bool isInfiniteLoop = true, CancellationToken? cancellationToken = null)
        {
            return await ProcessMessagesAsync(_logsTopicName, isInfiniteLoop, cancellationToken);
        }

        protected override async Task ProcessItemAsync(LogMessageDto dto)
        {
            LogEntity entity;
            try
            {
                using var _ = _dbSessionProvider;
                var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(dto.ApplicationId);
                if (application == null)
                {
                    await LogMessageModel(dto, "Application not found for the log message model!");
                    return;
                }

                // 2. Save log
                entity = dto.GetEntity();
                entity.Application = application;
                await _commandBuilder.SaveAsync(entity);
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
                messageModel.ApplicationId.ToString()
            );
            await _commandBuilder.SaveAsync(request);
            _logger.LogError(logMessage);
        }
    }
}
