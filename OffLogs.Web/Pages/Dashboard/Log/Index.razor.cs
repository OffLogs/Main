using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Shared.Ui.Table;
using OffLogs.Web.Store.Log;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Pages.Dashboard.Log;

public partial class Index
{
    [Inject]
    private IApiService _apiService { get; set; }
    
    [Inject]
    private ToastService _toastService { get; set; }
    
    [Inject]
    private IState<LogsListState> _logListState { get; set; }
    
    private CustomAsyncDropDown _dropDownApplications;
    private LogListItemDto _selectedLog = null;
    private List<long> _expandedLogIds = new();

    private long? _selectedApplicationId;
    private LogLevel? _selectedLogLevel;

    private ICollection<CustomTableRowModel> _tableCols = new List<CustomTableRowModel>()
    {
        new(){ Name = "Message" },
        new(){ Name = "Log time" },
        new(){ Name = "Create time" }
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task LoadListAsync(bool isLoadNextPage = true)
    {
        if (!isLoadNextPage)
        {
            Dispatcher.Dispatch(new ResetListAction()); 
        }
        Dispatcher.Dispatch(new FetchNextListPageAction()
        {
            ApplicationId = _selectedApplicationId.Value,
            LogLevel = _selectedLogLevel
        });
    }

    private Task OnClickRowAsync(LogListItemDto log)
    {
        _selectedLog = log;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task OnMoreAsyncAsync()
    {
        await LoadListAsync();
    }

    private async Task OnSelectedApplication(DropDownListItem selectListItem)
    {
        _selectedApplicationId = selectListItem.IdAsLong;
        await LoadListAsync(false);
    }

    private async Task<ICollection<DropDownListItem>> OnLoadApplicationsAsync()
    {
        try
        {
            var response = await _apiService.GetApplicationsAsync();
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

