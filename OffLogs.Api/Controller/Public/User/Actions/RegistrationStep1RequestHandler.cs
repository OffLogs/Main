using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.Extensions.Configuration;
using Notification.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class RegistrationStep1RequestHandler : IAsyncRequestHandler<RegistrationStep1Request>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IUserService _userService;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly string _frontendUrl;

        public RegistrationStep1RequestHandler(
            IAsyncQueryBuilder queryBuilder, 
            IUserService userService,
            IKafkaProducerService kafkaProducerService,
            IConfiguration configuration
        )
        {
            _queryBuilder = queryBuilder;
            _userService = userService;
            _kafkaProducerService = kafkaProducerService;
            _frontendUrl = configuration.GetValue<string>("App:FrontendUrl");
        }

        public async Task ExecuteAsync(RegistrationStep1Request request)
        {
            var currentUser = _queryBuilder.For<UserGetByQuery>()
                .WithAsync(new UserGetByCriteria(null, request.Email));
            if (currentUser != null)
            {
                throw new Exception("User is exists");
            }

            var createdUser = await _userService.CreatePendingUser(request.Email);
            await _kafkaProducerService.ProduceNotificationMessageAsync(
                new RegistrationNotificationContext(
                    createdUser.Email, 
                    _frontendUrl,
                    createdUser.VerificationToken
                )
            );
        }
    }
}