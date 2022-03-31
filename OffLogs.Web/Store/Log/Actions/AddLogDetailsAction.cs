using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class AddLogDetailsAction
{
    public LogDto Log { get; }

    public AddLogDetailsAction(LogDto log)
    {
        Log = log;
    }
}
