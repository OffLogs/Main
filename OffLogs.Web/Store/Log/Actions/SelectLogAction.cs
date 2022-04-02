using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class SelectLogAction
{
    public long? Id { get; }

    public SelectLogAction(long? id)
    {
        Id = id;
    }
}
