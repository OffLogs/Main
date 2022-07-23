using Domain.Abstractions;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Monetization;

public interface IRestrictionValidationService: IDomainService
{
    void CheckNotificationRulesAddingAvailable(UserEntity user);
}
