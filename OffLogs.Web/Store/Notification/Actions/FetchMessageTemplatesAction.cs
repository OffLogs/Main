namespace OffLogs.Web.Store.Notification.Actions;

public class FetchMessageTemplatesAction
{
    public bool IsLoadIfEmpty { get; }

    public FetchMessageTemplatesAction(bool isLoadIfEmpty = false)
    {
        IsLoadIfEmpty = isLoadIfEmpty;
    }
}
