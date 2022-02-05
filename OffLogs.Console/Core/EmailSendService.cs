using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Business.Services.Kafka;
using OffLogs.Console.Verbs;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Console.Core
{
    public class EmailSendService: IEmailSendService
    {
        private readonly ILogger<EmailSendService> _logger;
        private readonly IUserService _userService;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IDbSessionProvider _dbSessionProvider;
        private readonly IKafkaProducerService _kafkaProducerService;

        public EmailSendService(
            ILogger<EmailSendService> logger,
            IUserService userService,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IDbSessionProvider dbSessionProvider,
            IKafkaProducerService kafkaProducerService
        )
        {
            _logger = logger;
            _userService = userService;
            _queryBuilder = queryBuilder;
            _applicationService = applicationService;
            _dbSessionProvider = dbSessionProvider;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task EmailSend(EmailSendVerb verb)
        {
            if (string.IsNullOrEmpty(verb.Email))
            {
                throw new ArgumentNullException(nameof(verb.Email));
            }

            await _kafkaProducerService.ProduceNotificationMessageAsync(
                new TestNotificationContext(verb.Email)
            );
        }
    }
}
