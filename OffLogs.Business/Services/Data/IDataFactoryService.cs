using Bogus;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Business.Services.Data
{
    public interface IDataFactoryService
    {
        Faker<UserEntity> UserFactory();
        Faker<UserEmailEntity> UserEmailFactory();
        Faker<ApplicationEntity> ApplicationFactory(UserEntity user);
        Faker<LogEntity> LogFactory(LogLevel level);
        Faker<LogTraceEntity> LogTraceFactory();
        Faker<LogPropertyEntity> LogPropertyFactory();
        Faker<NotificationConditionEntity> NotificationConditionFactory();
        Faker<MessageTemplateEntity> MessageTemplateFactory();
        Faker<NotificationRuleEntity> NotificationRuleFactory();
    }
}
