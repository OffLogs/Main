using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Log;

[FeatureState]
public class LogsListState
{
    public bool IsLoadingList { get; }
    
    public int Page { get; }
    
    public bool HasMoreItems { get; }

    public ICollection<LogListItemDto> List { get; } = new List<LogListItemDto>();
    
    public ICollection<LogDto> LogsDetails { get; } = new List<LogDto>();

    public LogsListState() { }

    public LogsListState(ICollection<LogListItemDto> list)
    {
        List = list;
    }
    
    public LogsListState(LogsListState state, ICollection<LogDto> logsDetails)
    {
        LogsDetails = logsDetails;

        IsLoadingList = state.IsLoadingList;
        Page = state.Page;
        HasMoreItems = state.HasMoreItems;
        List = state.List;
    }

    public LogsListState(
        bool isLoadingList,
        int? page = null,
        ICollection<LogListItemDto> list = null,
        bool? hasMoreItems = null
    )
    {
        IsLoadingList = isLoadingList;
        if (list != null)
        {
            List = list;
            
        }
        if (page.HasValue)
        {
            Page = page.Value;
        }
        if (hasMoreItems.HasValue)
        {
            HasMoreItems = hasMoreItems.Value;
        }
    }
}
