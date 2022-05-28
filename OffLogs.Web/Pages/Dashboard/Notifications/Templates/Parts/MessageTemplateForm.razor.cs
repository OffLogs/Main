using System;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Business.Common.Resources;
using OffLogs.Web.Resources;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;
using OffLogs.Web.Store.Notification;
using Radzen;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Templates.Parts;

public partial class MessageTemplateForm
{
    [Inject]
    private IState<NotificationRuleState> _state { get; set; }
    
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public long Id { get; set; }

    private SetMessageTemplateRequest _model = new();
    private MyButton _btnSubmit;
    private MyEditForm _editForm;
    private bool _isLoading = false;
    private bool _isShowDeleteModal = false;

    private bool _isNew => Id == 0;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (_model.Id != Id)
        {
            var foundItem = _state.Value.MessageTemplates.FirstOrDefault(
                item => item.Id == Id
            );
            if (foundItem == null)
            {
                foundItem = new MessageTemplateDto()
                {
                    Subject = GNotificationResources.MessageTemplate_Default_Subject,
                    Body = GNotificationResources.MessageTemplate_Default_Body
                };
            }
            _model.Fill(foundItem);
        }
    }
    
    public void Delete()
    {
        if (_isNew)
        {
            return;
        }

        _isShowDeleteModal = true;
    }
    
    private async Task HandleSubmit()
    {
        _isLoading = true;
        try
        {
            var item = await ApiService.MessageTemplateSet(_model);
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = _isNew ? NotificationResources.MessageTemplate_Added : NotificationResources.MessageTemplate_Saved    
            });
            Dispatcher.Dispatch(new SetMessageTemplatesAction(item));
            DialogService.Close();
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
