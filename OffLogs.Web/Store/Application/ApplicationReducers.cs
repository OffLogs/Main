using System.Linq;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Extensions;

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
        newState.SkipItems = 0;
        newState.List.Clear();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceFetchListResultActionAction(ApplicationsListState state, FetchListResultAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.IsLoading = false;
        newState.HasMoreItems = action.IsHasMore;
        newState.TotalCount = action.TotalCount;
        newState.List = state.List.Concat(action.Items).ToList();
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceRemoveApplicationFromListAction(ApplicationsListState state, RemoveApplicationFromListAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.List = state.List.Where(
            i => i.Id != action.Id
        ).ToList();
        newState.SelectedApplicationId = null;
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceAddApplicationListItemAction(ApplicationsListState state, AddApplicationListItemAction action)
    {
        var newState = state with {};
        newState.List.Add(new ApplicationListItemDto
        {
            Id = action.Item.Id,
            CreateTime = action.Item.CreateTime,
            Name = action.Item.Name,
            UserId = action.Item.UserId
        });
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceSetPaginationInfoAction(ApplicationsListState state, SetPaginationInfoAction action)
    {
        var newState = state.JsonClone<ApplicationsListState>();
        newState.SkipItems = action.SkipItems;
        return newState;
    }
    
    [ReducerMethod]
    public static ApplicationsListState ReduceUpdateApplicationActionAction(ApplicationsListState state, UpdateApplicationAction action)
    {
        var newState = state with { };
        foreach (var app in newState.List)
        {
            if (app.Id == action.Application.Id)
            {
                app.Name = action.Application.Name;
            }
        }
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
