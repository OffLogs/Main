using System.Collections.Generic;
using Fluxor;
using OffLogs.Web.Core.Models.Toast;

namespace OffLogs.Web.Store.Shared.Toast;

[FeatureState]
public class ToastMessagesState
{
    public ICollection<ToastMessageModel> Messages { get; }

    public ToastMessagesState()
    {
        Messages = new List<ToastMessageModel>();
    }

    public ToastMessagesState(ICollection<ToastMessageModel> messages)
    {
        Messages = messages;
    }
}
