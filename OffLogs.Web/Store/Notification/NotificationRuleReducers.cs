using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Store.Notification;

public class ApplicationReducers
{
    [ReducerMethod(typeof(FetchMessageTemplatesAction))]
    public static NotificationRuleState ReduceFetchMessageTemplatesAction(NotificationRuleState state)
    {
        var newState = state.JsonClone<NotificationRuleState>();
        newState.IsLoading = true;
        return newState;
    }
    
    public static NotificationRuleState ReduceFetchMessageTemplatesResultAction(
        FetchMessageTemplatesResultAction action,
        NotificationRuleState state
    )
    {
        var newState = state.JsonClone<NotificationRuleState>();
        newState.IsLoading = false;
        newState.MessageTemplates = action.List?.Items ?? new List<MessageTemplateDto>();
        return newState;
    }
    
    [ReducerMethod(typeof(FetchNotificationRulesAction))]
    public static NotificationRuleState ReduceFetchNotificationRulesAction(NotificationRuleState state)
    {
        var newState = state.JsonClone<NotificationRuleState>();
        newState.IsLoading = true;
        return newState;
    }
    
    public static NotificationRuleState ReduceFetchNotificationRulesResultAction(
        FetchNotificationRulesResultAction action,
        NotificationRuleState state
    )
    {
        var newState = state.JsonClone<NotificationRuleState>();
        newState.IsLoading = false;
        newState.Rules = action.List?.Items ?? new List<NotificationRuleDto>();
        return newState;
    }
}
