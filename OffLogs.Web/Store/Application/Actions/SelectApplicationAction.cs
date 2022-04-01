namespace OffLogs.Web.Store.Application.Actions;

public class SelectApplicationAction
{
    public long? ApplicationId  { get; }

    public SelectApplicationAction(long? applicationId)
    {
        ApplicationId = applicationId;
    }
}
