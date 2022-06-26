using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Common.Exceptions.Common;
using OffLogs.Business.Extensions;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.UserEmails;
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Settings.UserEmails;

public partial class UserEmails
{
    [Inject]
    private IState<UserEmailsState> State { get; set; }
    
    [Inject]
    private ILogger<UserEmails> Logger { get; set; }

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
        await _grid.EditRow(State.Value.ItemToAdd);
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

    private async Task DeleteRow(UserEmailDto item)
    {
        var isOk = await DialogService.Confirm(
            UserResources.DeleteConfirmation,
            CommonResources.DeletionConfirmation,
            new ConfirmOptions()
            {
                OkButtonText = "Ok",
                CancelButtonText = CommonResources.Cancel
            }
        );
        if (isOk.HasValue && isOk.Value)
        {
            try
            {
                await ApiService.UserEmailDeleteAsync(item.Id);

                Dispatcher.Dispatch(new FetchListAction());
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = CommonResources.Error_ServerError
                });
            }
        }
    }

    private async Task OnUpdateRow(UserEmailDto item)
    {
        Dispatcher.Dispatch(new RemoveEmailFromFromListAction());
        try
        {
            await ApiService.UserEmailAddAsync(item.Email);

            Dispatcher.Dispatch(new FetchListAction());
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = UserResources.EmailWasAdded
            });
        }
        catch (HttpResponseException exception)
        {
            var error = CommonResources.Error_ServerError;
            if (DomainException.TooManyRecordsException.EqualsToTypeName(exception.Type))
            {
                error = UserResources.Error_TooManyEmails;
            }
            if (DomainException.RecordIsExistsException.EqualsToTypeName(exception.Type))
            {
                error = UserResources.Error_RecordIsExists;
            }
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = error
            });
        }
    }
    #endregion
}
