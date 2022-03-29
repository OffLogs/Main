using System.Collections.Generic;
using System.Text.Json.Serialization;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application.Actions;

public class FetchListResultAction
{
    public ICollection<ApplicationListItemDto> Items { get; }

    public long TotalPages { get; }

    public int PageSize { get; }

    public long TotalCount { get; }
    
    public bool IsHasMore { get; }

    public FetchListResultAction(
        ICollection<ApplicationListItemDto> items,
        long totalPages,
        long totalCount,
        int pageSize,
        bool isHasMore
    )
    {
        Items = items;
        TotalPages = totalPages;
        TotalCount = totalCount;
        PageSize = pageSize;
        IsHasMore = isHasMore;
    }
}
