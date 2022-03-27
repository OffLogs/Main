using System;
using System.Linq;
using Fluxor;
using OffLogs.Web.Core.Models.Toast;
using OffLogs.Web.Store.Shared.Toast.Actions;

namespace OffLogs.Web.Store.Shared.Toast;

public class ToastMessagesReducers
{
    [ReducerMethod]
    public static ToastMessagesState ReduceAddMessageAction(
        ToastMessagesState state,
        AddMessageAction action
    )
    {
        var messages = state.Messages.ToList();
        var toastModel = new ToastMessageModel()
        {
            Type = action.Type,
            Title = action.Title,
            Content = action.Content,
            CreatedAt = DateTime.UtcNow
        };
        messages.Add(toastModel);
        return new ToastMessagesState(messages);
    }
    
    [ReducerMethod]
    public static ToastMessagesState ReduceRemoveMessageAction(
        ToastMessagesState state,
        RemoveMessageAction action
    )
    {
        state.Messages.Remove(action.Message);
        return new ToastMessagesState(state.Messages);
    }
}
