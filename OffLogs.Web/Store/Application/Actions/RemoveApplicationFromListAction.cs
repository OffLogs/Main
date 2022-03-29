using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application.Actions;

public class RemoveApplicationFromListAction
{
    public long Id { get; }

    public RemoveApplicationFromListAction(long id)
    {
        Id = id;
    }
}
