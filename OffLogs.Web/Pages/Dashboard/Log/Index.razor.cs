using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Extensions;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Log;
using OffLogs.Web.Store.Log.Models;
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
    
    private long? _selectedApplicationId;

    private ICollection<HeaderMenuButton> _menuButtons = new List<HeaderMenuButton>();

    private string _search;
    
    private RadzenDataGrid<LogListItemDto> _grid;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        State.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(object sender, EventArgs e)
    {
        SetLogMenuButtons();
    }
    
    private void SetLogMenuButtons()
    {
        _menuButtons.Clear();
        if (State.Value.SelectedLog != null)
        {
            _menuButtons.Add(
                State.Value.SelectedLog.IsFavorite
                    ? new(LogResources.UnSetFavorite, "checked", () => SetIsFavorite(false))
                    : new(LogResources.SetFavorite, "check", () => SetIsFavorite(true))
            );
        }
        _menuButtons.Add(
            new(LogResources.ShowStatistic, "chart-arrows-axis", ShowStatisticModal)    
        );
    }

    private void SetIsFavorite(bool isFavorite)
    {
        Dispatcher.Dispatch(new SetIsLogFavoriteAction(State.Value.SelectedLog.Id, isFavorite));
    }


    private async Task OnApplicationSelected(ApplicationListItemDto app)
    {
        _selectedApplicationId = app?.Id;
        Dispatcher.Dispatch(new SetApplication(app?.Id ?? 0));
        await _grid.GoToPage(0);
        Dispatcher.Dispatch(new FetchListPageAction());
    }
    
    private Task OnClickIsFavoriteAsync(LogListItemDto log)
    {
        Dispatcher.Dispatch(new SetIsLogFavoriteAction(log.Id, !log.IsFavorite));
        return Task.CompletedTask;
    }
    
    private void ShowStatisticModal()
    {
        if (_selectedApplicationId.HasValue)
        {
            _isShowStatistic = true;    
        }
    }

    private Task OnLoadList(LoadDataArgs arg)
    {
        Dispatcher.Dispatch(new FetchListPageAction(arg.Skip ?? 0));
        return Task.CompletedTask;
    }

    private void ShowInfoModal(LogListItemDto value)
    {
        throw new NotImplementedException();
    }
}

