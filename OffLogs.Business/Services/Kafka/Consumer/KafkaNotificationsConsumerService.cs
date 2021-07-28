using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notification.Abstractions;
using OffLogs.Business.Notifications;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka.Models;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Kafka.Consumer
{
    public partial class KafkaNotificationsConsumerService 
        : KafkaConsumerService<NotificationMessageDto>, IKafkaNotificationsConsumerService
    {
        private readonly string _logsTopicName;
        private readonly IAsyncNotificationBuilder _notificationBuilder;

        public KafkaNotificationsConsumerService(
            IConfiguration configuration,
            ILogger<IKafkaProducerService> logger,
            IAsyncNotificationBuilder notificationBuilder
        ) : base(
            configuration,
            logger
        )
        {
            ConsumerName = "NotificationsConsumer";
            _logsTopicName = configuration.GetValue<string>("Kafka:Topic:Notifications");
            _notificationBuilder = notificationBuilder;
        }

        public async Task<long> ProcessNotificationsAsync(CancellationToken cancellationToken)
        {
            return await ProcessMessagesAsync(_logsTopicName, true, cancellationToken);
        }

        public async Task<long> ProcessNotificationsAsync(bool isInfiniteLoop = true, CancellationToken? cancellationToken = null)
        {
            return await ProcessMessagesAsync(_logsTopicName, isInfiniteLoop, cancellationToken);
        }

        protected override async Task ProcessItemAsync(NotificationMessageDto dto)
        {
            try
            {
                var contextType = GetContextType(dto);
                var notificationContext = dto.GetDeserializedData(contextType);
                await _notificationBuilder.SendAsync(contextType);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
            await Task.CompletedTask;
        }

        private Type GetContextType(NotificationMessageDto dto)
        {
            var activationResult = Activator.CreateInstance(
                typeof(BusinessNotificationsAssemblyMarker).Assembly.GetName().Name,
                dto.ContextType
            );
            return activationResult.Unwrap().GetType();
        }
    }
}