using System;
using Bogus;
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
        
        public Faker<ApplicationEntity> ApplicationFactory(int userId)
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
    }
}