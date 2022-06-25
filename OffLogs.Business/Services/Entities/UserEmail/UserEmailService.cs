using System;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Helpers;
using OffLogs.Business.Notifications.Senders.User;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Exceptions;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Criterias;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.UserEmail
{
    public class UserEmailService: IUserEmailService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly ILogger<UserEmailService> _logger;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IKafkaProducerService _kafkaProducerService;

        private readonly string _frontendUrl;
        
        public UserEmailService(
            IAsyncCommandBuilder commandBuilder, 
            ILogger<UserEmailService> logger,
            IAsyncQueryBuilder queryBuilder,
            IKafkaProducerService kafkaProducerService,
            IConfiguration configuration
        )
        {
            _commandBuilder = commandBuilder;
            _logger = logger;
            _queryBuilder = queryBuilder;
            _kafkaProducerService = kafkaProducerService;
            _frontendUrl = configuration.GetValue<string>("App:FrontendUrl");
        }

        public async Task<UserEmailEntity> AddAsync(UserEntity user, string email)
        {
            var userEmail = new UserEmailEntity
            {
                Email = email,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                VerificationToken = SecurityUtil.GetTimeBasedToken()
            };

            if (user.Emails.Any(item => item.Email == userEmail.Email))
            {
                throw new RecordIsExistsException();
            }
            userEmail.SetUser(user);
            if (user.Emails.Count > GlobalConstants.MaxUserEmailsCount)
            {
                throw new TooManyRecordsException();
            }
            await _commandBuilder.SaveAsync(user);
            
            await _kafkaProducerService.ProduceNotificationMessageAsync(
                new EmailVerificationNotificationContext(
                    userEmail.Email,
                    _frontendUrl,
                    userEmail.VerificationToken
                )
            );
            
            return userEmail;
        }
        
        public async Task<UserEmailEntity> VerifyByTokenAsync(string token)
        {
            var emailEntity = await _queryBuilder.For<UserEmailEntity>()
                .WithAsync(new GetByTokenCriteria(token));
            if (emailEntity == null || emailEntity.IsVerified)
            {
                throw new RecordNotFoundException();
            }
            emailEntity.VerificationTime = DateTime.UtcNow;
            emailEntity.VerificationToken = null;
            await _commandBuilder.SaveAsync(emailEntity);

            await _kafkaProducerService.ProduceNotificationMessageAsync(
                new EmailVerifiedNotificationContext(emailEntity.User.Email, emailEntity.Email)
            );

            return emailEntity;
        }
    }
}
