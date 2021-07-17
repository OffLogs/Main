using Bogus;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Entities;

namespace OffLogs.Business.Services.Data
{
    public interface IDataFactoryService
    {
        Faker<UserEntity> UserFactory();
        Faker<ApplicationEntity> ApplicationFactory(UserEntity user);
        Faker<LogEntity> LogFactory(LogLevel level);
        Faker<LogTraceEntity> LogTraceFactory();
        Faker<LogPropertyEntity> LogPropertyFactory();
    }
}