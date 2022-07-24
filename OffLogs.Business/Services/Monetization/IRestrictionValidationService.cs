using Domain.Abstractions;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Monetization;

public interface IRestrictionValidationService: IDomainService
{
    void CheckNotificationRulesAddingAvailable(NotificationRuleEntity newRule);

    void CheckUserEmailsAddingAvailable(UserEntity user);

    void CheckApplicationsAddingAvailable(UserEntity user);
}
