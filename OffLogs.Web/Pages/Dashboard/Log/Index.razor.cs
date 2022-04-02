using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Log;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Pages.Dashboard.Log;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private IState<LogsListState> State { get; set; }
    
    [Inject]
    private IState<ApplicationsListState> ApplicationsState { get; set; }
    
    private CustomAsyncDropDown _dropDownApplications;
    private List<long> _expandedLogIds = new();
    private bool _isShowStatistic = false;
    
    private long? _selectedApplicationId;
    private LogLevel? _selectedLogLevel;

    private ICollection<HeaderMenuButton> _menuButtons = new List<HeaderMenuButton>();

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
            return State.Value.List.Select(
                log => new ListItem()
                {
                    Id = log.Id.ToString(),
                    SubTitle = log.Message.Truncate(32),
                    RightTitle = log.LogTime.ToString("MM/dd/yyyy hh:mm tt"),
                    Title = log.Level.GetLabel()
                }
            ).ToList();
        }
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _menuButtons.Add(
            new(LogResources.ShowStatistic, "chart-arrows-axis", () => _isShowStatistic = true)    
        );
        
        Dispatcher.Dispatch(new OffLogs.Web.Store.Application.Actions.FetchNextListPageAction());
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
        Dispatcher.Dispatch(new SetListFilterAction(
            _selectedApplicationId.Value,
            _selectedLogLevel
        ));
        await LoadListAsync(false);
    }

    private async Task OnSelectedApplication(DropDownListItem selectListItem)
    {
        await LoadListAsync(false);
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

    private void ExpandOrCloseLog(LogListItemDto log)
    {
        if (_expandedLogIds.Contains(log.Id))
        {
            _expandedLogIds.Remove(log.Id);
        }
        else
        {
            _expandedLogIds.Add(log.Id);
        }
        StateHasChanged();
    }

    private async Task OnSelectLogLevelAsync(LogLevel level)
    {
        _selectedLogLevel = null;
        if (level != default)
        {
            _selectedLogLevel = level;
        }
        StateHasChanged();
        await LoadListAsync(false);
    }    
    
    private Task OnClickIsFavoriteAsync(LogListItemDto log)
    {
        Dispatcher.Dispatch(new SetIsLogFavoriteAction(log.Id, !log.IsFavorite));
        return Task.CompletedTask;
    }
}

