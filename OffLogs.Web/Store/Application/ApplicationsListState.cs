using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application;

[FeatureState]
public class ApplicationsListState
{
    public bool IsLoading { get; }
    
    public int Page { get; }
    
    public bool HasMoreItems { get; }

    public ICollection<ApplicationListItemDto> Applications { get; } = new List<ApplicationListItemDto>();

    public ApplicationsListState() { }

    public ApplicationsListState(
        ICollection<ApplicationListItemDto> applications
    )
    {
        Applications = applications;
    }

    public ApplicationsListState(
        bool isLoading,
        int? page = null,
        ICollection<ApplicationListItemDto> applications = null,
        bool? hasMoreItems = null
    )
    {
        IsLoading = isLoading;
        if (applications != null)
        {
            Applications = applications;
            
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
