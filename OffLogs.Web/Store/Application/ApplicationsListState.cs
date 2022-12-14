using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Application;

[FeatureState]
public record ApplicationsListState
{
    public int PageSize { get; set; } = GlobalConstants.ListPageSize;
    
    public bool IsLoading { get; set; }

    public int TotalCount { get; set; }
    
    public bool HasMoreItems { get; set; }

    public ICollection<ApplicationListItemDto> List { get; set; } = new List<ApplicationListItemDto>();

    public ICollection<ApplicationListItemDto> SortedList
    {
        get
        {
            var query = List.AsQueryable();
            if (HasItemToAdd)
            {
                query = query.OrderBy(item => item.Id);
            }
            return query.ToList();
        }
    }

    public int SkipItems { get; set; } = 0;
    
    public bool HasItemToAdd
    {
        get => ItemToAdd != null;
    }
    
    public ApplicationListItemDto ItemToAdd
    {
        get => List.FirstOrDefault(item => item.Id == 0);
    }

    public ApplicationsListState() { }
}
