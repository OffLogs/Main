using System.Collections.Generic;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Log;

public class LogReducers
{
    #region Logs list
    
    [ReducerMethod]
    public static LogsListState ReduceFetchApplicationListAction(LogsListState state, FetchListPageAction action)
    {
        var newState = state with {};
        newState.SkipItems = action.SkipItems;
        newState.IsLoadingList = true;
        return newState;
    }
    
    [ReducerMethod(typeof(ResetListAction))]
    public static LogsListState ReduceResetListAction(LogsListState state)
    {
        return state with
        {
            IsLoadingList = false,
            SkipItems = 0,
            TotalCount = 0,
            List = new List<LogListItemDto>(),
            SelectedLog = null
        };
    }
    
    [ReducerMethod]
    public static LogsListState ReduceFetchListResultAction(LogsListState state, FetchListResultAction action)
    {
        return state with
        {
            IsLoadingList = false,
            List = action.Data.Items,
            HasMoreItems = action.Data.IsHasMore,
            TotalCount = action.Data.TotalCount
        };
    }
    
    [ReducerMethod]
    public static LogsListState ReduceSetIsLogFavoriteAction(LogsListState state, SetIsLogFavoriteAction action)
    {
        var newState = state with {};
        newState.IsLoadingList = false;
        foreach (var log in newState.List)
        {
            if (log.Id == action.LogId)
            {
                log.IsFavorite = action.IsFavorite;
            }
        }

        if (newState.SelectedLog?.Id == action.LogId)
        {
            newState.SelectedLog.IsFavorite = action.IsFavorite;
        }

        return newState;
    }
    
    [ReducerMethod]
    public static LogsListState ReduceSelectLogAction(LogsListState state, SelectLogAction action)
    {
        var newState = state with {};
        newState.SelectedLog = newState.List.FirstOrDefault(log => log.Id == action.Id);
        return newState;
    }
    
    [ReducerMethod]
    public static LogsListState ReduceSetListFilterAction(LogsListState state, SetListFilterAction action)
    {
        var newState = state with
        {
            Filter = action.Filter
        };
        return newState;
    }

    [ReducerMethod]
    public static LogsListState ReduceSetListFilterSearchAction(LogsListState state, SetListFilterSearchAction action)
    {
        var newState = state with {};
        newState.Filter = state.Filter with
        {
            Search = action.Search
        };
        return newState;
    }
    
    [ReducerMethod(typeof(UpdateFilteredItemsAction))]
    public static LogsListState ReduceSetListFilterSearchAction(LogsListState state)
    {
        var newState = state with {};
        newState.FilteredList = state.List.Where(item =>
        {
            if (string.IsNullOrEmpty(state.Filter.Search))
            {
                return true;
            }

            return item.Message.Contains(state.Filter.Search);
        }).ToList();
        return newState;
    }
    #endregion
    
    #region LogDetails
    [ReducerMethod]
    public static LogsListState ReduceAddLogDetailsAction(LogsListState state, AddLogDetailsAction action)
    {
        var newState = state with {};
        newState.LogsDetails.Add(action.Log);
        return newState;
    }
    
    [ReducerMethod]
    public static LogsListState ReduceResetLogDetailsAction(LogsListState state, ResetLogDetailsAction action)
    {
        var newState = state with {};
        newState.LogsDetails = new List<LogDto>();
        return newState;
    }
    #endregion
}
