namespace OffLogs.Web.Store.Application.Actions;

public class FetchNextListPageAction
{
    public bool IsLoadIfEmpty { get; }

    public FetchNextListPageAction(bool isLoadIfEmpty = false)
    {
        IsLoadIfEmpty = isLoadIfEmpty;
    }
}
