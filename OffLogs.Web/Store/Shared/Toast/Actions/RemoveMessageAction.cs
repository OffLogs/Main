using OffLogs.Web.Constants;
using OffLogs.Web.Core.Models.Toast;

namespace OffLogs.Web.Store.Shared.Toast.Actions;

public class RemoveMessageAction
{
    public ToastMessageModel Message { get; }

    public RemoveMessageAction(ToastMessageModel message)
    {
        Message = message;
    }
}
