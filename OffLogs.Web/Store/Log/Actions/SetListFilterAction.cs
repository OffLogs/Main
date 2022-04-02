using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class SetListFilterAction
{
    public long ApplicationId { get; }
    
    public LogLevel? LogLevel { get; }

    public SetListFilterAction(long applicationId, LogLevel? logLevel = null)
    {
        ApplicationId = applicationId;
        LogLevel = logLevel;
    }
}
