using System;
using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Helpers;

namespace OffLogs.Web.Store.Application;

public class ApplicationReducers
{
    [ReducerMethod(typeof(FetchListPageAction))]
    public static ApplicationsListState ReduceFetchListPageAction(ApplicationsListState state)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.IsLoading = true;
        newState.SkipItems = state.SkipItems;
        return newState;
    }
    
    [ReducerMethod(typeof(ResetListAction))]
    public static ApplicationsListState ReduceResetListAction(ApplicationsListState state)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.IsLoading = false;
        newState.SkipItems = 0;
        newState.SortedList.Clear();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceFetchListResultActionAction(ApplicationsListState state, FetchListResultAction action)
    {
        return state with
        {
            IsLoading = false,
            HasMoreItems = action.IsHasMore,
            TotalCount = action.TotalCount,
            List = action.Items
        };
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceRemoveApplicationFromListAction(ApplicationsListState state, RemoveApplicationFromListAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.List = state.SortedList.Where(
            i => i.Id != action.Id
        ).ToList();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceAddApplicationListItemAction(ApplicationsListState state, AddApplicationListItemAction action)
    {
        state.List.Add(new ApplicationListItemDto
        {
            Id = action.Item.Id,
            CreateTime = action.Item.CreateTime,
            Name = action.Item.Name,
            UserId = action.Item.UserId
        });
        return state with {};
    }

    [ReducerMethod]
    public static ApplicationsListState ReduceUpdateApplicationActionAction(ApplicationsListState state, UpdateApplicationAction action)
    {
        var newState = state with { };
        foreach (var app in newState.SortedList)
        {
            if (app.Id == action.Application.Id)
            {
                app.Name = action.Application.Name;
            }
        }
        return newState;
    }

    #region Add new item
    
    [ReducerMethod(typeof(AddApplicationToAddListItemAction))]
    public static ApplicationsListState ReduceAddApplicationToAddListItemAction(ApplicationsListState state)
    {
        var newList = state.SortedList.ToList();
        newList.Add(new ApplicationListItemDto()
        {
            Id = 0
        });
        return state with
        {
            List = newList
        };
    }
    
    [ReducerMethod(typeof(RemoveApplicationToAddListItemAction))]
    public static ApplicationsListState ReduceRemoveApplicationToAddListItemAction(ApplicationsListState state)
    {
        return state with
        {
            List = state.List.Where(item => item.Id != 0).ToList()
        };
    }
    
    #endregion
}
