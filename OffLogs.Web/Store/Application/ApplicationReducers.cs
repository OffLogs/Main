using Fluxor;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Application;

public class ApplicationReducers
{
    [ReducerMethod]
    public static ApplicationsListState ReduceFetchApplicationListAction(
        ApplicationsListState state,
        FetchApplicationListAction action
    )
    {
        var page = action.IsLoadNext ? state.Page + 1 : 0;
        return new(true, page);
    }
}
