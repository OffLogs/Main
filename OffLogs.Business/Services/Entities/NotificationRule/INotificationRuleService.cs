using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Business.Services.Entities.NotificationRule;

public interface INotificationRuleService: IDomainService
{
    Task<NotificationRuleEntity> GetNextAndSetExecutingAsync();

    Task<NotificationRuleEntity> CreateRule(
        UserEntity user,
        int period,
        LogicOperatorType logicOperator,
        NotificationMessageEntity message,
        ICollection<NotificationConditionEntity> conditions,
        ApplicationEntity application = null
    );

    Task<ProcessingDataDto> GetDataForNotificationRule(NotificationRuleEntity rule);
}
