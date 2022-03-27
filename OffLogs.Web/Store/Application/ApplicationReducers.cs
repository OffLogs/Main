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
    public static ApplicationsListState ReduceResetListAction()
    {
        return new ApplicationsListState(false, 0);
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceResetListAction(ApplicationsListState state, FetchListResultAction action)
    {
        if (action.Result == null)
        {
            return new ApplicationsListState(false);
        }
        return new ApplicationsListState(
            false,
            state.Page,
            action.Result.Items
        );
    }
}
