using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class FetchLogAction
{
    public long LogId { get; }

    public FetchLogAction(long logId)
    {
        LogId = logId;
    }
}
