using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Pages.Dashboard.Notifications.Templates.Parts;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.Notification;
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Templates;

public partial class Index
{
    [Inject]
    private IState<NotificationRuleState> State { get; set; }
    
    private RadzenDataGrid<MessageTemplateDto> _grid;
    
    private bool _isLoading = false;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(new FetchMessageTemplatesAction());
    }
    
    private async Task OnTemplateSelected(long id)
    {
        if (id == 0)
        {
            // Item unselected
            return;
        }

        await ShowEditModal(id);
        await _grid.SelectRow(null);
    }
    
    private async Task AddNewTemplate()
    {
        await ShowEditModal(0);
    }
    
    private async Task DeleteRowAsync(MessageTemplateDto value)
    {
        var isOk = await DialogService.Confirm(
            NotificationResources.Confirmation_AreSureToDeleteTemplate,
            CommonResources.DeletionConfirmation,
            new ConfirmOptions()
            {
                OkButtonText = "Ok",
                CancelButtonText = CommonResources.Cancel
            }
        );
        if (!isOk.HasValue || !isOk.Value)
        {
            return;
        }
        
        _isLoading = true;
        try
        {
            await ApiService.MessageTemplateDelete(value.Id);
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = NotificationResources.MessageTemplate_Deleted
            });
            Dispatcher.Dispatch(new DeleteMessageTemplatesAction(value.Id));
        }
        catch (Exception e)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = NotificationResources.MessageTemplate_CantDelete    
            });
        }
        finally
        {
            _isLoading = false;
        }
        StateHasChanged();    
    }

    private async Task ShowEditModal(long id)
    {
        await DialogService.OpenAsync<MessageTemplateForm>(
            id == 0 ? NotificationResources.AddNewTemplate : NotificationResources.EditTemplate,
            new Dictionary<string, object> { { "Id", id } },
            new DialogOptions { Width = "700px", Height = "570px", Resizable = true, Draggable = false }
        );
    }
}
