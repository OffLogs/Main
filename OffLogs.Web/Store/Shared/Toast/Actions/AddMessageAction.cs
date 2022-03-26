using OffLogs.Web.Constants;

namespace OffLogs.Web.Store.Shared.Toast.Actions;

public class AddMessageAction
{
    public ToastMessageType Type { get; }
    public string Title { get; }
    public string Content { get; }

    public AddMessageAction(ToastMessageType type, string title, string content)
    {
        Content = content;
        Title = title;
        Type = type;
    }
}
