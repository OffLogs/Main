using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Store.Log.Models;

namespace OffLogs.Web.Store.Log;

[FeatureState]
public record LogsListState
{
    public int PageSize { get; set; } = GlobalConstants.ListPageSize * 2;
    
    public bool IsLoadingList { get; set; }
    
    public int SkipItems { get; set; }
    
    public int TotalCount { get; set; }
    
    public bool HasMoreItems { get; set; }

    public ICollection<LogListItemDto> List { get; set; } = new List<LogListItemDto>();
    
    public ICollection<LogListItemDto> FilteredList { get; set; } = new List<LogListItemDto>();
    
    public LogListItemDto SelectedLog { get; set; }
    
    public ICollection<LogDto> LogsDetails { get; set; } = new List<LogDto>();

    public long ApplicationId { get; set; }
    
    public LogFilterModel Filter { get; set; } = new();

    public LogsListState() { }

    public LogsListState(ICollection<LogListItemDto> list)
    {
        List = list;
    }
}
