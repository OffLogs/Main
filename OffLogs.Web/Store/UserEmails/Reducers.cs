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
        var newState = state with {};
        newState.IsLoading = true;
        return newState;
    }
    
    [ReducerMethod]
    public static UserEmailsState ReduceFetchListResultActionAction(UserEmailsState state, FetchListResultAction action)
    {
        return state with
        {
            IsLoading = false,
            List = action.Items
        };
    }
    
    [ReducerMethod]
    public static UserEmailsState ReduceSetIsLoadingAction(UserEmailsState state, SetIsLoadingAction action)
    {
        return state with
        {
            IsLoading = action.IsLoading
        };
    }
    
    [ReducerMethod(typeof(AddEmailToAddListItemAction))]
    public static UserEmailsState ReduceAddApplicationToAddListItemAction(UserEmailsState state)
    {
        var newList = state.List.ToList();
        newList.Add(new UserEmailDto()
        {
            Id = 0
        });
        return state with
        {
            List = newList
        };
    }
    
    [ReducerMethod]
    public static UserEmailsState ReduceRemoveEmailFromFromListAction(UserEmailsState state, RemoveEmailFromFromListAction action)
    {
        var newState = state with { };
        newState.List = state.List.Where(
            i => i.Id != action.Id
        ).ToList();
        return newState;
    }
}
