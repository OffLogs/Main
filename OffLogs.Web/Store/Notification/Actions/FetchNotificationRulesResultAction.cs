using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification.Actions;

public class FetchNotificationRulesResultAction
{
    public ListDto<NotificationRuleDto> List { get; }

    public FetchNotificationRulesResultAction(ListDto<NotificationRuleDto> list)
    {
        List = list;
    }
}
