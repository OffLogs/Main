using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Store.Log;

public class LogReducers
{
    #region Logs list
    
    [ReducerMethod(typeof(FetchNextListPageAction))]
    public static LogsListState ReduceFetchApplicationListAction(LogsListState state)
    {
        return new LogsListState(true, state.Page + 1);
    }
    
    [ReducerMethod(typeof(ResetListAction))]
    public static LogsListState ReduceResetListAction(LogsListState state)
    {
        return new LogsListState(false, 0);
    }
    
    [ReducerMethod]
    public static LogsListState ReduceFetchListResultActionAction(LogsListState state, FetchListResultAction action)
    {
        if (action.Data.Items == null)
        {
            return new LogsListState(false);
        }
        return new LogsListState(
            false,
            state.Page,
            action.Data.Items,
            action.Data.IsHasMore
        );
    }
    
    #endregion
    
    #region LogDetails
    [ReducerMethod]
    public static LogsListState ReduceAddLogDetailsActionAction(LogsListState state, AddLogDetailsAction action)
    {
        state.LogsDetails.Add(action.Log);
        return new LogsListState(state, state.LogsDetails);
    }
    
    [ReducerMethod]
    public static LogsListState ReduceResetLogDetailsActionAction(LogsListState state, ResetLogDetailsAction action)
    {
        return new LogsListState(state, new List<LogDto>());
    }
    #endregion
}
