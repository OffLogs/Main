using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application.Actions;

public class FetchListResultAction
{
    public PaginatedListDto<ApplicationListItemDto> Result { get; }

    public FetchListResultAction(PaginatedListDto<ApplicationListItemDto> result)
    {
        Result = result;
    }
}
