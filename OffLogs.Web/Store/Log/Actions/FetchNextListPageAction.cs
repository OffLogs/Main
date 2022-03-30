using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class FetchNextListPageAction
{
    public long ApplicationId { get; set; }
    
    public LogLevel? LogLevel { get; set; }
}
