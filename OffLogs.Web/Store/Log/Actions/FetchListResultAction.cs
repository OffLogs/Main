using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class FetchListResultAction
{
    public PaginatedListDto<LogListItemDto> Data { get; }

    public FetchListResultAction(PaginatedListDto<LogListItemDto> data)
    {
        Data = data;
    }
}
