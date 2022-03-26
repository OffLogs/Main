using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application;

[FeatureState]
public class ApplicationsListState
{
    public bool IsLoading { get; }
    
    public int Page { get; }
    
    public ICollection<ApplicationListItemDto> Applications { get; }

    public ApplicationsListState() { }
    
    public ApplicationsListState(
        bool isLoading,
        int page = 0,
        ICollection<ApplicationListItemDto> applications = null
    )
    {
        IsLoading = isLoading;
        if (applications != null)
        {
            Applications = applications;
            Page = page;    
        }
    }
}
