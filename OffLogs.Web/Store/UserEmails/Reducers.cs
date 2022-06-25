using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Extensions;

namespace OffLogs.Web.Store.UserEmails;

public class Reducers
{
    [ReducerMethod(typeof(FetchListAction))]
    public static UserEmailsState ReduceFetchListAction(UserEmailsState state)
    {
        var newState = state.JsonClone<UserEmailsState>();
        newState.IsLoading = true;
        return newState;
    }
    
    [ReducerMethod]
    public static UserEmailsState ReduceFetchListResultActionAction(UserEmailsState state, FetchListResultAction action)
    {
        return state with
        {
            IsLoading = false,
            TotalCount = action.TotalCount,
            List = action.Items
        };
    }
}
