using System;
using Bogus;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;

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
                    entity => entity.PasswordHash,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.PasswordSalt,
                    (faker) => faker.Random.Bytes(32)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past()
                );
        }
        
        public Faker<ApplicationEntity> ApplicationFactory(long userId)
        {
            return new Faker<ApplicationEntity>()
                .RuleFor(
                    entity => entity.Name,
                    (faker) => faker.Internet.DomainName()
                )
                .RuleFor(
                    entity => entity.UserId,
                    () => userId
                )
                .RuleFor(
                    entity => entity.ApiToken,
                    (faker) => faker.Random.String2(100)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past()
                );
        }
        
        public Faker<LogEntity> LogFactory(long applicationId, LogLevel level)
        {
            return new Faker<LogEntity>()
                .RuleFor(
                    entity => entity.ApplicationId,
                    () => applicationId
                )
                .RuleFor(
                    entity => entity.Message,
                    (faker) => faker.Lorem.Sentence()
                )
                .RuleFor(
                    entity => entity.Level,
                    () => level
                )
                .RuleFor(
                    entity => entity.LogTime,
                    (faker) => faker.Date.Past()
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past()
                );
        }

        public Faker<LogTraceEntity> LogTraceFactory(long logId)
        {
            return new Faker<LogTraceEntity>()
                .RuleFor(
                    entity => entity.LogId,
                    () => logId
                )
                .RuleFor(
                    entity => entity.Trace,
                    (faker) => faker.Lorem.Sentence()
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past()
                );
        }
        
        public Faker<LogPropertyEntity> LogPropertyFactory(long logId)
        {
            return new Faker<LogPropertyEntity>()
                .RuleFor(
                    entity => entity.LogId,
                    () => logId
                )
                .RuleFor(
                    entity => entity.Key,
                    (faker) => faker.Random.Word()
                )
                .RuleFor(
                    entity => entity.Value,
                    (faker) => faker.Random.Word()
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past()
                );
        }
    }
}