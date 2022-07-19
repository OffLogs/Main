using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Entities.NotificationRule;

public interface INotificationRuleService: IDomainService
{
    Task<NotificationRuleEntity> GetNextAndSetExecutingAsync(CancellationToken cancellationToken = default);

    Task<NotificationRuleEntity> SetRule(
        UserEntity user,
        string title,
        int period,
        LogicOperatorType logicOperator,
        NotificationType type,
        MessageTemplateEntity messageTemplate,
        ICollection<NotificationConditionEntity> conditions,
        ApplicationEntity application = null,
        long? existsRuleId = null,
        ICollection<UserEmailEntity> additionalEmails = null
    );

    Task<ProcessingDataDto> GetDataForNotificationRule(NotificationRuleEntity rule);

    Task<NotificationRuleEntity> SetAsExecutedAsync(
        NotificationRuleEntity rule,
        CancellationToken cancellationToken = default
    );
}
