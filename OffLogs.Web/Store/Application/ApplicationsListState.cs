using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application;

[FeatureState]
public class ApplicationsListState
{
    public bool IsLoading { get; set; }
    
    public int Page { get; set; }
    
    public bool HasMoreItems { get; set; }

    public ICollection<ApplicationListItemDto> List { get; set; } = new List<ApplicationListItemDto>();

    public long? SelectedApplicationId  { get; set; }
    
    public ApplicationsListState() { }
}
