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
    
    private ICollection<MenuItem> _menuItems
    {
        get
        {
            return ApplicationsState.Value.List.Select(
                application => new MenuItem()
                {
                    Id = application.Id.ToString(),
                    Title = application.Name
                }
            ).ToList();
        }
    }
    
    private ICollection<ListItem> _logsList
    {
        get
        {
            return State.Value.FilteredList.Select(
                log => new ListItem()
                {
                    Id = log.Id.ToString(),
                    SubTitle = log.Message.Truncate(32),
                    RightTitle = log.LogTime.ToString("MM/dd/yyyy hh:mm tt"),
                    Title = log.Level.GetLabel(),
                    TitleColorType = log.Level.GetBootstrapColorType(),
                    IconName = log.IsFavorite ? "check-alt" : ""
                }
            ).ToList();
        }
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SetLogMenuButtons();
        Dispatcher.Dispatch(new OffLogs.Web.Store.Application.FetchNextListPageAction());

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

    private Task LoadListAsync(bool isLoadNextPage = true)
    {
        if (!isLoadNextPage)
        {
            Dispatcher.Dispatch(new ResetListAction());
        }
        Dispatcher.Dispatch(new FetchNextListPageAction());
        return Task.CompletedTask;
    }

    private async Task OnApplicationSelected(OnSelectEventArgs menuEvent)
    {
        _selectedApplicationId = long.Parse(menuEvent.MenuItem.Id);
        Dispatcher.Dispatch(new SetApplication(_selectedApplicationId.Value));
        await LoadListAsync(false);
    }

    private async Task OnSelectedApplication(DropDownListItem selectListItem)
    {
        await LoadListAsync(false);
    }

    private async Task OnSelectLogAsync(OnSelectEventArgs menuEvent)
    {
        Dispatcher.Dispatch(new SelectLogAction(
            long.Parse(menuEvent.ListItem.Id)    
        ));
        await Task.CompletedTask;
    }
    
    private async Task<ICollection<DropDownListItem>> OnLoadApplicationsAsync()
    {
        try
        {
            var response = await ApiService.GetApplicationsAsync();
            return response.Items.Select(record => new DropDownListItem()
            {
                Id = $"{record.Id}",
                Label = record.Name
            }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message, e);
        }
        StateHasChanged();
        return default;
    }
    
    private Task OnClickIsFavoriteAsync(LogListItemDto log)
    {
        Dispatcher.Dispatch(new SetIsLogFavoriteAction(log.Id, !log.IsFavorite));
        return Task.CompletedTask;
    }
    
    private async Task OnClickMoreBtnAsync()
    {
        await LoadListAsync();
    }
    
    private void ShowStatisticModal()
    {
        if (_selectedApplicationId.HasValue)
        {
            _isShowStatistic = true;    
        }
    }
}

