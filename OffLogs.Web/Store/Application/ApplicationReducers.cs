using System.Linq;
using Fluxor;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Application;

public class ApplicationReducers
{
    [ReducerMethod(typeof(FetchNextListPageAction))]
    public static ApplicationsListState ReduceFetchApplicationListAction(ApplicationsListState state)
    {
        return new ApplicationsListState(true, state.Page + 1);
    }
    
    [ReducerMethod(typeof(ResetListAction))]
    public static ApplicationsListState ReduceResetListAction(ApplicationsListState state)
    {
        return new ApplicationsListState(false, 0);
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceFetchListResultActionAction(ApplicationsListState state, FetchListResultAction action)
    {
        if (action.Items == null)
        {
            return new ApplicationsListState(false);
        }
        return new ApplicationsListState(
            false,
            state.Page,
            action.Items,
            action.IsHasMore
        );
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceRemoveApplicationFromListAction(ApplicationsListState state, RemoveApplicationFromListAction action)
    {
        var applications = state.Applications.Where(
            i => i.Id != action.Id
        ).ToList();
        return new ApplicationsListState(applications);
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceAddApplicationActionAction(ApplicationsListState state, AddApplicationAction action)
    {
        state.Applications.Add(action.Application);
        return new ApplicationsListState(state.Applications);
    }
}
