using System;
using Bogus;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entities;

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
                    (faker) => faker.Date.Past()
                )
                .RuleFor(
                    entity => entity.UpdateTime,
                    (faker) => faker.Date.Past()
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
                    entity => entity.Message,
                    (faker) => faker.Lorem.Sentence()
                )
                .RuleFor(
                    entity => entity.Level,
                    () => level
                )
                .RuleFor(
                    entity => entity.LogTime,
                    (faker) => DateTime.Now.AddMinutes(-1)
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => DateTime.Now
                );
        }

        public Faker<LogTraceEntity> LogTraceFactory()
        {
            return new Faker<LogTraceEntity>()
                .RuleFor(
                    entity => entity.Trace,
                    (faker) => faker.Lorem.Sentence()
                )
                .RuleFor(
                    entity => entity.CreateTime,
                    (faker) => faker.Date.Past()
                );
        }
        
        public Faker<LogPropertyEntity> LogPropertyFactory()
        {
            return new Faker<LogPropertyEntity>()
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