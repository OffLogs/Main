using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notification.Abstractions;
using OffLogs.Business.Notifications;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Notifications.Senders.NotificationRule;
using OffLogs.Business.Notifications.Senders.User;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka.Models;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Kafka.Consumer
{
    public partial class KafkaNotificationsConsumerService 
        : KafkaConsumerService<NotificationMessageDto>, IKafkaNotificationsConsumerService
    {
        private readonly string _topicName;
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
            _topicName = configuration.GetValue<string>("Kafka:Topic:Notifications");
            _notificationBuilder = notificationBuilder;
        }

        public async Task<long> ProcessNotificationsAsync(CancellationToken cancellationToken)
        {
            return await ProcessMessagesAsync(_topicName, true, cancellationToken);
        }

        public async Task<long> ProcessNotificationsAsync(bool isInfiniteLoop = true, CancellationToken? cancellationToken = null)
        {
            return await ProcessMessagesAsync(_topicName, isInfiniteLoop, cancellationToken);
        }

        protected override async Task ProcessItemAsync(NotificationMessageDto dto)
        {
            try
            {
                var errorMessage = $"Incorrect notification context: {dto.ContextType}";
                var contextType = GetContextType(dto);
                if (contextType == null)
                {
                    throw new Exception(errorMessage);
                }
                if (IsContext<RegularLogsNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<RegularLogsNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<LogsDeletedNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<LogsDeletedNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<ApplicationDeletedNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<ApplicationDeletedNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<RegistrationNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<RegistrationNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<TestNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<TestNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<EmailNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<EmailNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<EmailVerifiedNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<EmailVerifiedNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                if (IsContext<EmailVerificationNotificationContext>(contextType))
                {
                    var context = dto.GetDeserializedData<EmailVerificationNotificationContext>();
                    await _notificationBuilder.SendAsync(context);
                    return;
                }
                throw new Exception(errorMessage);
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

        private bool IsContext<TConext>(Type contextType) where TConext: INotificationContext
        {
            return contextType == typeof(TConext);
        }
    }
}
