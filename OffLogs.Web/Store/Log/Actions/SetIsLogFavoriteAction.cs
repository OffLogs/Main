using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Actions;

public class SetIsLogFavoriteAction
{
    public long LogId { get; }
    
    public bool IsFavorite { get; }

    public SetIsLogFavoriteAction(long logId, bool isFavorite)
    {
        LogId = logId;
        IsFavorite = isFavorite;
    }
}
