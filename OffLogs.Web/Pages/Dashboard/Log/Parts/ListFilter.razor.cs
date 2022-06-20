using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Extensions;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Store.Log;
using OffLogs.Web.Store.Log.Models;

namespace OffLogs.Web.Pages.Dashboard.Log.Parts;

public partial class ListFilter
{
    [Parameter]
    public EventCallback OnFilterChanged { get; set; }
    
    [Inject]
    private IState<LogsListState> State { get; set; }

    private LogFilterModel _model;

    private LogFilterModel _clearModel
    {
        get => new()
        {
            ApplicationId = _model.ApplicationId,
            CreateTimeFrom = DateTime.Now.AddDays(-5).StartOfDay()
        };
    }

    private LogLevel _logLevel
    {
        get => _model.LogLevel ?? default;
        set => _model.LogLevel = value;
    }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            if (State.Value.Filter.HasValue)
            {
                _model = State.Value.Filter.Value;
            }
            else
            {
                _model = _clearModel;
                if (NavigationManager.TryGetQueryValue<long>("application-id", out var applicationId))
                {
                    _model.ApplicationId = applicationId;
                }
                if (NavigationManager.TryGetQueryValue<LogLevel>("level", out var logLevel))
                {
                    _model.LogLevel = logLevel;
                }
                if (NavigationManager.TryGetQueryValue<bool>("is-favorite", out var isFavorite))
                {
                    _model.IsOnlyFavorite = isFavorite;
                }

                await OnSave();
            }
        }
    }

    private async Task OnSave()
    {
        Dispatcher.Dispatch(new SetListFilterAction(_model));
        await InvokeAsync(() => OnFilterChanged.InvokeAsync());
    }
    
    private async Task Reset()
    {
        _model = _clearModel;
        await OnSave();
    }
}
