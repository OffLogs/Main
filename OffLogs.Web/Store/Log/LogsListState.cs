using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Helpers;

namespace OffLogs.Web.Store.Log;

[FeatureState]
public class LogsListState
{
    public bool IsLoadingList { get; set; }
    
    public int Page { get; set; }
    
    public bool HasMoreItems { get; set; }

    public ICollection<LogListItemDto> List { get; set; } = new List<LogListItemDto>();
    
    public ICollection<LogDto> LogsDetails { get; set; } = new List<LogDto>();

    public LogsListState() { }

    public LogsListState(ICollection<LogListItemDto> list)
    {
        List = list;
    }
    
    public LogsListState(
        bool isLoadingList,
        int page,
        ICollection<LogListItemDto> list,
        bool hasMoreItems,
        ICollection<LogDto> logsDetails
    )
    {
        IsLoadingList = isLoadingList;
        List = list;
        Page = page;
        HasMoreItems = hasMoreItems;
        LogsDetails = logsDetails;
    }

    public LogsListState Clone()
    {
        return this.JsonClone<LogsListState>();
    }
}
