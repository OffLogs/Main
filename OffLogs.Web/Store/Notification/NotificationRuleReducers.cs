using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Helpers;

namespace OffLogs.Web.Store.Notification;

public class ApplicationReducers
{
    #region Message Templates

    [ReducerMethod(typeof(FetchMessageTemplatesAction))]
    public static NotificationRuleState ReduceFetchMessageTemplatesAction(NotificationRuleState state)
    {
        var newState = state with {};;
        newState.IsLoading = true;
        return newState;
    }
    
    [ReducerMethod]
    public static NotificationRuleState ReduceFetchMessageTemplatesResultAction(
        NotificationRuleState state,
        FetchMessageTemplatesResultAction action
    )
    {
        var newState = state with {};;
        newState.IsLoading = false;
        newState.MessageTemplates = action.List?.Items
                .OrderBy(item => item.Id)
                .ToList() 
            ?? new List<MessageTemplateDto>();
        return newState;
    }

    [ReducerMethod]
    public static NotificationRuleState ReduceSetMessageTemplatesAction(
        NotificationRuleState state,
        SetMessageTemplatesAction action
    )
    {
        var oldList = state.MessageTemplates.Where(
            item => item.Id != action.Item.Id
        ).ToList();
        var newState = state with {};;
        newState.MessageTemplates = oldList;
        newState.MessageTemplates.Add(action.Item);
        newState.MessageTemplates = newState.MessageTemplates.OrderBy(item => item.Id).ToList();
        return newState;
    }
    
    [ReducerMethod]
    public static NotificationRuleState ReduceDeleteMessageTemplatesAction(
        NotificationRuleState state,
        DeleteMessageTemplatesAction action
    )
    {
        var newState = state with {};;
        newState.MessageTemplates = state.MessageTemplates.Where(
            item => item.Id != action.Id
        ).ToList();
        return newState;
    }
    #endregion

    #region Rules

    [ReducerMethod(typeof(FetchNotificationRulesAction))]
    public static NotificationRuleState ReduceFetchNotificationRulesAction(NotificationRuleState state)
    {
        var newState = state with {};;
        newState.IsLoading = true;
        return newState;
    }
    
    [ReducerMethod]
    public static NotificationRuleState ReduceFetchNotificationRulesResultAction(
        NotificationRuleState state,
        FetchNotificationRulesResultAction action
    )
    {
        var newState = state with {};;
        newState.IsLoading = false;
        newState.Rules = action.List?.Items ?? new List<NotificationRuleDto>();
        return newState;
    }

    [ReducerMethod]
    public static NotificationRuleState ReduceSetMessageTemplatesAction(
        NotificationRuleState state,
        SetNotificationRuleAction action
    )
    {
        var oldList = state.Rules.Where(
            item => item.Id != action.Item.Id
        ).ToList();
        var newState = state with {};;
        newState.Rules = oldList;
        newState.Rules.Add(action.Item);
        newState.Rules = newState.Rules.OrderBy(item => item.Id).ToList();
        return newState;
    }
    
    [ReducerMethod]
    public static NotificationRuleState ReduceDeleteMessageTemplatesAction(
        NotificationRuleState state,
        DeleteNotificationRuleAction action
    )
    {
        var newState = state with {};;
        newState.Rules = state.Rules.Where(
            item => item.Id != action.Id
        ).ToList();
        return newState;
    }
    #endregion
}
 
