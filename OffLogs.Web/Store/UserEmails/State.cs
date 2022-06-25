using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.UserEmails;

[FeatureState]
public record UserEmailsState
{
    public int PageSize { get; set; } = GlobalConstants.ListPageSize;
    
    public bool IsLoading { get; set; }

    public int TotalCount { get; set; }
    
    public ICollection<UserEmailDto> List { get; set; }

    public UserEmailsState() { }
}
