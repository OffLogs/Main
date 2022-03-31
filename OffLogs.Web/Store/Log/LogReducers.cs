using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Store.Log;

public class LogReducers
{
    #region Logs list
    
    [ReducerMethod(typeof(FetchNextListPageAction))]
    public static LogsListState ReduceFetchApplicationListAction(LogsListState state)
    {
        var newState = state.Clone();
        newState.Page += 1;
        newState.IsLoadingList = true;
        return newState;
    }
    
    [ReducerMethod(typeof(ResetListAction))]
    public static LogsListState ReduceResetListAction(LogsListState state)
    {
        var newState = state.Clone();
        newState.IsLoadingList = false;
        newState.Page = 0;
        newState.List = new List<LogListItemDto>();
        return newState;
    }
    
    [ReducerMethod]
    public static LogsListState ReduceFetchListResultAction(LogsListState state, FetchListResultAction action)
    {
        var newState = state.Clone();
        newState.IsLoadingList = false;
        newState.List = newState.List.Concat(action.Data.Items).ToList();
        newState.HasMoreItems = action.Data.IsHasMore;
        return newState;
    }
    
    [ReducerMethod]
    public static LogsListState ReduceSetIsLogFavoriteAction(LogsListState state, SetIsLogFavoriteAction action)
    {
        var newState = state.Clone();
        newState.IsLoadingList = false;
        foreach (var log in newState.List)
        {
            if (log.Id == action.LogId)
            {
                log.IsFavorite = action.IsFavorite;
            }
        }

        return newState;
    }
    
    #endregion
    
    #region LogDetails
    [ReducerMethod]
    public static LogsListState ReduceAddLogDetailsAction(LogsListState state, AddLogDetailsAction action)
    {
        var newState = state.Clone();
        newState.LogsDetails.Add(action.Log);
        return newState;
    }
    
    [ReducerMethod]
    public static LogsListState ReduceResetLogDetailsAction(LogsListState state, ResetLogDetailsAction action)
    {
        var newState = state.Clone();
        newState.LogsDetails = new List<LogDto>();
        return newState;
    }
    #endregion
}
