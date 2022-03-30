using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Log;

[FeatureState]
public class LogsListState
{
    public bool IsLoading { get; }
    
    public int Page { get; }
    
    public bool HasMoreItems { get; }

    public ICollection<LogListItemDto> Logs { get; } = new List<LogListItemDto>();

    public LogsListState() { }

    public LogsListState(ICollection<LogListItemDto> logs)
    {
        Logs = logs;
    }

    public LogsListState(
        bool isLoading,
        int? page = null,
        ICollection<LogListItemDto> logs = null,
        bool? hasMoreItems = null
    )
    {
        IsLoading = isLoading;
        if (logs != null)
        {
            Logs = logs;
            
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
