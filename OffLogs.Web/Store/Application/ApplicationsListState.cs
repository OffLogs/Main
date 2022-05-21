using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Application;

[FeatureState]
public record ApplicationsListState
{
    public bool IsLoading { get; set; }
    
    public int Page { get; set; }
    
    public int TotalCount { get; set; }
    
    public bool HasMoreItems { get; set; }

    public ICollection<ApplicationListItemDto> List { get; set; } = new List<ApplicationListItemDto>();

    public int SkipItems { get; set; } = 0;
    
    public ICollection<ApplicationListItemDto> PaginatedList
    {
        get => List.Skip(SkipItems).Take(GlobalConstants.ListPageSize).ToList();
    }

    public long? SelectedApplicationId  { get; set; }
    
    public ApplicationsListState() { }
}
