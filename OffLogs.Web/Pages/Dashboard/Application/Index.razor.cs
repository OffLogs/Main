using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.Application;
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Application;

public partial class Index
{
    [Inject]
    private IState<ApplicationsListState> State { get; set; }

    private  RadzenDataGrid<ApplicationListItemDto> _grid;

    private ApplicationListItemDto _applicationToInsert = null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        Dispatcher.Dispatch(new FetchNextListPageAction());
    }

    private Task OnLoadApplicationsList(LoadDataArgs arg)
    {
        if (State.Value.HasMoreItems)
        {
            Dispatcher.Dispatch(new FetchNextListPageAction());    
        }
        Dispatcher.Dispatch(new SetPaginationInfoAction(arg.Skip ?? 0));
        return Task.CompletedTask;
    }

    private async Task UpdateApplication(ApplicationListItemDto app)
    {
        try
        {
            var application = await ApiService.UpdateApplicationAsync(app.Id, app.Name);
            Dispatcher.Dispatch(new UpdateApplicationAction(application));
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = ApplicationResources.ApplicationWasUpdated
            });
        }
        catch (Exception e)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = e.Message
            });
        }
    }

    #region Grid
    
    private async Task InsertRow()
    {
        _applicationToInsert = new ApplicationListItemDto();
        Debug.Log(_applicationToInsert);
        await _grid.InsertRow(_applicationToInsert);
    }
    
    private async Task EditRow(ApplicationListItemDto app)
    {
        await _grid.EditRow(app);
    }

    private async Task OnClickSaveRow(ApplicationListItemDto app)
    {
        await _grid.UpdateRow(app);
    }

    private void OnClickCancelEditMode(ApplicationListItemDto app)
    {
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
        }
        _grid.CancelEditRow(app);
    }

    private async Task DeleteRow(ApplicationListItemDto app)
    {
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
            _grid.CancelEditRow(app);
            return;
        }
        
        var isOk = await DialogService.Confirm(
            ApplicationResources.DeleteConfirmation,
            CommonResources.DeletionConfirmation,
            new ConfirmOptions()
            {
                OkButtonText = "Ok",
                CancelButtonText = CommonResources.Cancel
            }
        );
        if (isOk.HasValue && isOk.Value)
        {
            Dispatcher.Dispatch(new DeleteApplicationAction(
                app.Id
            ));
        }
    }

    private async Task OnUpdateRow(ApplicationListItemDto app)
    {
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
        }
        await UpdateApplication(app);
    }

    private Task OnCreateRow(ApplicationListItemDto app)
    {
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
        }
        Dispatcher.Dispatch(new AddApplicationAction(app.Name));
        NotificationService.Notify(new NotificationMessage()
        {
            Severity = NotificationSeverity.Info,
            Summary = "New application was added"
        });
        return Task.CompletedTask;
    }
    
    #endregion
}
