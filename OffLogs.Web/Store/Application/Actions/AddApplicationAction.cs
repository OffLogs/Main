using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application.Actions;

public class AddApplicationAction
{
    public ApplicationListItemDto Application { get; }

    public AddApplicationAction(ApplicationListItemDto application)
    {
        Application = application;
    }
}
