using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.UserEmails;
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Settings.UserEmails;

public partial class UserEmails
{
    [Inject]
    private IState<UserEmailsState> State { get; set; }

    private  RadzenDataGrid<UserEmailDto> _grid;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(new FetchListAction());
    }
    
    #region Grid
    
    private async Task InsertRow()
    {
        Dispatcher.Dispatch(new AddEmailToAddListItemAction());
        await EditRow(State.Value.ItemToAdd);
    }
    
    private async Task EditRow(UserEmailDto app)
    {
        await _grid.EditRow(app);
    }

    private async Task OnClickSaveRow(UserEmailDto app)
    {
        await _grid.UpdateRow(app);
    }

    private void OnClickCancelEditMode(UserEmailDto app)
    {
        Dispatcher.Dispatch(new RemoveEmailFromFromListAction());
        _grid.CancelEditRow(app);
    }

    private async Task DeleteRow(UserEmailDto app)
    {
        if (State.Value.HasItemToAdd)
        {
            Dispatcher.Dispatch(new RemoveEmailFromFromListAction());
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
            // Dispatcher.Dispatch(new DeleteApplicationAction(
            //     app.Id
            // ));
        }
    }

    private async Task OnUpdateRow(UserEmailDto app)
    {
        Dispatcher.Dispatch(new RemoveEmailFromFromListAction());
        if (app.Id > 0)
        {
            // await UpdateApplication(app);
            return;
        }

        Dispatcher.Dispatch(new RemoveEmailFromFromListAction());
        // Dispatcher.Dispatch(new AddApplicationAction(app.Name));
        NotificationService.Notify(new NotificationMessage()
        {
            Severity = NotificationSeverity.Info,
            Summary = "New application was added"
        });
    }
    #endregion
}
