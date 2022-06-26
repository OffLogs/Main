using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.UserEmails;

[FeatureState]
public record UserEmailsState
{
    public bool IsLoading { get; set; }

    private ICollection<UserEmailDto> _list = new List<UserEmailDto>();
    public ICollection<UserEmailDto> List {
        get
        {
            var query = _list.AsQueryable();
            if (HasItemToAdd)
            {
                query = query.OrderBy(item => item.Id);
            }
            return query.ToList();
        }
        set => _list = value;
    }

    public bool HasItemToAdd
    {
        get => ItemToAdd != null;
    }
    
    public UserEmailDto ItemToAdd
    {
        get => _list.FirstOrDefault(item => item.Id == 0);
    }
    
    public UserEmailsState() { }
}
