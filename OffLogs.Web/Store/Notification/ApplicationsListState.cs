using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification;

[FeatureState]
public class NotificationsListState
{
    public bool IsLoading { get; set; }

    public ICollection<NotificationRuleDto> Rules { get; set; } = new List<NotificationRuleDto>();

    public ICollection<NotificationRuleDto> MessageTemplates { get; set; } = new List<NotificationRuleDto>();
    
    public NotificationsListState() { }
}
