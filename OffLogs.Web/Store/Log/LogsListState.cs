using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
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
    
    public LogListItemDto SelectedLog { get; set; }
    
    public ICollection<LogDto> LogsDetails { get; set; } = new List<LogDto>();

    #region Filter

    public long ApplicationId { get; set; }
    
    public LogLevel? LogLevel { get; set; }

    #endregion
    
    public LogsListState() { }

    public LogsListState(ICollection<LogListItemDto> list)
    {
        List = list;
    }

    public LogsListState Clone()
    {
        return this.JsonClone<LogsListState>();
    }
}
