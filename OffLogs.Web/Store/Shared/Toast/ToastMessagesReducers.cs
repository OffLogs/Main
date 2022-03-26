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
        var toastModel = new ToastMessageModel()
        {
            Type = action.Type,
            Title = action.Title,
            Content = action.Content,
            CreatedAt = DateTime.UtcNow
        };
        state.Messages.Add(toastModel);
        return new ToastMessagesState(state.Messages);
    }
    
    [ReducerMethod]
    public static ToastMessagesState ReduceRemoveMessageAction(
        ToastMessagesState state,
        RemoveMessageAction action
    )
    {
        if (action.Message == null)
        {
            var tempMessage = state.Messages.ToList();
            foreach (var message in tempMessage)
            {
                var timeDifference = DateTime.UtcNow - message.CreatedAt;
                if (timeDifference.Seconds >= 5)
                {
                    state.Messages.Remove(message);
                }
            }    
        }
        else
        {
            state.Messages.Remove(action.Message);
        }
        return new ToastMessagesState(state.Messages);
    }
}
