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
    
    private bool _isFilterOpened { get; set; }

    private LogFilterModel _model;
    
    private ICollection<DropDownListItem> _logLevelDownListItems => LogLevel.Warning.ToDropDownList();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _model = State.Value.Filter;
    }

    private void OnChangeLogLevel(DropDownListItem item)
    {
        if (item == null)
        {
            _model.LogLevel = null;
            return;
        }

        Enum.TryParse<LogLevel>(item?.Id, out var logLevel);
        _model.LogLevel = logLevel;
    }
    
    private async Task OnSave()
    {
        _isFilterOpened = false;
        Dispatcher.Dispatch(new SetListFilterAction(_model));
        await InvokeAsync(() => OnFilterChanged.InvokeAsync());
    }
}
