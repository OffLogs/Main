using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application.Actions;

public class DeleteApplicationAction
{
    public long Id { get; }

    public DeleteApplicationAction(long id)
    {
        Id = id;
    }
}
