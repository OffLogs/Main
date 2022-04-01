using System.Linq;
using Fluxor;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Application;

public class ApplicationReducers
{
    [ReducerMethod(typeof(FetchNextListPageAction))]
    public static ApplicationsListState ReduceFetchApplicationListAction(ApplicationsListState state)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.IsLoading = true;
        newState.Page = state.Page + 1;
        return newState;
    }
    
    [ReducerMethod(typeof(ResetListAction))]
    public static ApplicationsListState ReduceResetListAction(ApplicationsListState state)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.IsLoading = false;
        newState.Page = 0;
        newState.Applications.Clear();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceFetchListResultActionAction(ApplicationsListState state, FetchListResultAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.IsLoading = false;
        newState.HasMoreItems = action.IsHasMore;
        newState.Applications = state.Applications.Concat(action.Items).ToList();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceRemoveApplicationFromListAction(ApplicationsListState state, RemoveApplicationFromListAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.Applications = state.Applications.Where(
            i => i.Id != action.Id
        ).ToList();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceAddApplicationActionAction(ApplicationsListState state, AddApplicationAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.Applications.Add(action.Application);
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceSelectApplicationAction(ApplicationsListState state, SelectApplicationAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.SelectedApplicationId = action.ApplicationId;
        return newState;
    }
}
