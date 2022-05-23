using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Store.Log;
using OffLogs.Web.Store.Log.Models;

namespace OffLogs.Web.Pages.Dashboard.Log.Parts;

public partial class ListFilter
{
    [Inject]
    private IState<LogsListState> State { get; set; }
    
    [Parameter]
    public EventCallback OnFilterChanged { get; set; }

    private LogFilterModel _model;

    private string _search
    {
        get => _model.Search;
        set
        {
            _model.Search = value;
            Dispatcher.Dispatch(new SetListFilterSearchAction(_model.Search));
            Dispatcher.Dispatch(new UpdateFilteredItemsAction());
        }
    }

    private bool _isApplied
    {
        get => _model.IsOnlyFavorite || _model.LogLevel.HasValue;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _model = State.Value.Filter;
    }

    private async Task OnSave()
    {
        Dispatcher.Dispatch(new SetListFilterAction(_model));
        await InvokeAsync(() => OnFilterChanged.InvokeAsync());
    }
    
    private async Task Reset()
    {
        _model = new LogFilterModel()
        {
            ApplicationId = _model.ApplicationId
        };
        await OnSave();
    }
}
