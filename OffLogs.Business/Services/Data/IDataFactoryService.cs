using Bogus;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Data
{
    public interface IDataFactoryService
    {
        Faker<UserEntity> UserFactory();
        Faker<ApplicationEntity> ApplicationFactory(UserEntity user);
        Faker<LogEntity> LogFactory(long applicationId, LogLevel level);
        Faker<LogTraceEntity> LogTraceFactory(long logId);
        Faker<LogPropertyEntity> LogPropertyFactory(long logId);
    }
}