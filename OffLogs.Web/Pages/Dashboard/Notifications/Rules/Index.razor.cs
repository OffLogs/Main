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
    
    private async Task OnItemSelectedAsync(NotificationRuleDto rule)
    {
        await DeleteRuleAsync(rule.Id);
        DialogService.Close();
    }
    
    private async Task ShowEditModal(long id)
    {
        await DialogService.OpenAsync<MessageTemplateForm>(
            id == 0 ? NotificationResources.AddNewTemplate : NotificationResources.EditTemplate,
            new Dictionary<string, object> { { "Id", id } },
            new DialogOptions { Width = "700px", Height = "570px", Resizable = true, Draggable = false }
        );
    }
    
    private async Task DeleteRuleAsync(long id)
    {
        _isLoading = true;
        try
        {
            await ApiService.NotificationRuleDelete(id);
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = NotificationResources.Rules_RuleDeleted
            });
            Dispatcher.Dispatch(new DeleteNotificationRuleAction(id));
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
        StateHasChanged();
    }
}
