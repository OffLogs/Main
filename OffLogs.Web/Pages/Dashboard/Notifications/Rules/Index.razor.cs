using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Pages.Dashboard.Notifications.Rules.Parts;
using OffLogs.Web.Pages.Dashboard.Notifications.Templates.Parts;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.Notification;
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Rules;

public partial class Index
{
    [Inject] 
    private IState<NotificationRuleState> _state { get; set; }
    
    private RadzenDataGrid<NotificationRuleDto> _grid;
    private bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        Dispatcher.Dispatch(new FetchNotificationRulesAction());
    }
    
    private async Task OnEditItemAsync(NotificationRuleDto rule)
    {
        if (rule == null)
        {
            // Un-select row
            return;
        }
        await ShowEditModal(rule.Id);
    }
    
    private async Task AddNewRule()
    {
        await ShowEditModal(0);
    }

    private async Task OnDeleteRule(NotificationRuleDto value)
    {
        var isOk = await DialogService.Confirm(
            NotificationResources.Confirmation_AreSureToDeleteRule,
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
            await ApiService.NotificationRuleDelete(value.Id);
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = NotificationResources.Rules_RuleDeleted
            });
            Dispatcher.Dispatch(new DeleteNotificationRuleAction(value.Id));
        }
        catch (Exception e)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = e.Message
            });
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    private async Task ShowEditModal(long id)
    {
        await DialogService.OpenAsync<EditRuleForm>(
            id == 0 ? NotificationResources.AddNewRule : NotificationResources.EditRule,
            new Dictionary<string, object> { { "Id", id } },
            new DialogOptions { Width = "700px", Height = "570px", Resizable = true, Draggable = false }
        );
    }
}
