using System;
using Bogus;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Data
{
    public class DataFactoryService: IDataFactoryService
    {
        public Faker<UserEntity> UserFactory()
        {
            return new Faker<UserEntity>()
                .RuleFor(
                    entity => entity.UserName,
                    (faker) => faker.Internet.UserName()
                )
                .RuleFor(
                    entity => entity.Email,
                    (faker) => faker.Internet.Email()
                )
                .RuleFor(
                    entity => entity.PublicKey,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.IsVerified,
                    () => true
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
        
        public Faker<UserEmailEntity> UserEmailFactory()
        {
            return new Faker<UserEmailEntity>()
                .RuleFor(
                    entity => entity.Email,
                    (faker) => faker.Internet.Email()
                )
                .RuleFor(
                    entity => entity.VerificationToken,
                    (faker) => SecurityUtil.GetTimeBasedToken()
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
        
        public Faker<ApplicationEntity> ApplicationFactory(UserEntity user)
        {
            return new Faker<ApplicationEntity>()
                .RuleFor(
                    entity => entity.Name,
                    (faker) => faker.Internet.DomainName()
                )
                .RuleFor(
                    entity => entity.ApiToken,
                    (faker) => faker.Random.String2(100)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.PublicKey,
                    (faker) => faker.Random.Bytes(100)
                )
                .RuleFor(
                    entity => entity.EncryptedPrivateKey,
                    (faker) => faker.Random.Bytes(100)
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.User,
                    (_) => user
                );
        }
        
        public Faker<LogEntity> LogFactory(LogLevel level)
        {
            return new Faker<LogEntity>()
                .RuleFor(
                    entity => entity.EncryptedMessage,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.Message,
                    (faker) => faker.Random.String2(32)
                )
                .RuleFor(
                    entity => entity.EncryptedSymmetricKey,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.Level,
                    () => level
                )
                .RuleFor(
                    entity => entity.LogTime,
                    (faker) => DateTime.UtcNow.AddMinutes(-1)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => DateTime.UtcNow
                );
        }

        public Faker<LogTraceEntity> LogTraceFactory()
        {
            return new Faker<LogTraceEntity>()
                .RuleFor(
                    entity => entity.EncryptedTrace,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.Trace,
                    (faker) => faker.Random.String2(32)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
        
        public Faker<LogPropertyEntity> LogPropertyFactory()
        {
            return new Faker<LogPropertyEntity>()
                .RuleFor(
                    entity => entity.EncryptedKey,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.EncryptedValue,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.Key,
                    (faker) => faker.Random.String2(32)
                )
                .RuleFor(
                    entity => entity.Value,
                    (faker) => faker.Random.String2(32)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
        
        public Faker<NotificationRuleEntity> NotificationRuleFactory()
        {
            return new Faker<NotificationRuleEntity>()
                .RuleFor(
                    entity => entity.Title,
                    (faker) => faker.Random.Words(2)
                )
                .RuleFor(
                    entity => entity.Type,
                    (faker) => NotificationType.Email
                )
                .RuleFor(
                    entity => entity.LogicOperator,
                    (faker) => LogicOperatorType.Disjunction
                )
                .RuleFor(
                    entity => entity.Period,
                    (faker) => faker.Random.Int(1, 100)
                )
                .RuleFor(
                    entity => entity.LastExecutionTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.IsExecuting,
                    (faker) => false
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
        
        public Faker<MessageTemplateEntity> MessageTemplateFactory()
        {
            return new Faker<MessageTemplateEntity>()
                .RuleFor(
                    entity => entity.Subject,
                    (faker) => faker.Lorem.Sentence()
                )
                .RuleFor(
                    entity => entity.Body,
                    (faker) => faker.Lorem.Sentence()
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
        
        public Faker<NotificationConditionEntity> NotificationConditionFactory()
        {
            return new Faker<NotificationConditionEntity>()
                .RuleFor(
                    entity => entity.ConditionField,
                    (faker) => ConditionFieldType.LogLevel
                )
                .RuleFor(
                    entity => entity.Value,
                    (faker) => "Error"
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past().ToUniversalTime()
                );
        }
    }
}
