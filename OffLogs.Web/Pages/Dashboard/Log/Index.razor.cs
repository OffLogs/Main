using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Pages.Dashboard.Log.Parts;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Log;
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Log;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private IState<LogsListState> State { get; set; }
    
    [Inject]
    private IState<OffLogs.Web.Store.Application.ApplicationsListState> ApplicationsState { get; set; }
    
    private bool _isShowStatistic = false;

    private ICollection<HeaderMenuButton> _menuButtons = new List<HeaderMenuButton>();

    private string _search;
    
    private RadzenDataGrid<LogListItemDto> _grid;

    private void SetIsFavorite(LogListItemDto log, bool isFavorite)
    {
        Dispatcher.Dispatch(new SetIsLogFavoriteAction(log.Id, isFavorite));
    }

    private Task OnLoadList(LoadDataArgs arg)
    {
        Dispatcher.Dispatch(new FetchListPageAction(arg.Skip ?? 0));
        return Task.CompletedTask;
    }

    private async Task ShowInfoModal(LogListItemDto log)
    {
        await DialogService.OpenAsync<LogInfoBlock>(
            LogResources.LogInfo,
            new Dictionary<string, object>() { { "LogId", log.Id } },
            new DialogOptions { Width = "700px", Height = "570px", Resizable = true, Draggable = false }
        );
    }

    private async Task OnFilterChanged(object arg)
    {
        await _grid.GoToPage(0);
        Dispatcher.Dispatch(new FetchListPageAction());
    }
}

