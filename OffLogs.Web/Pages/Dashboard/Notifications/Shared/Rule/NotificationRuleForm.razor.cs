using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Notification;
using Radzen;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Shared.Rule;

public partial class NotificationRuleForm
{
    [Inject]
    private IApiService _apiService { get; set; }

    [Inject]
    private IState<NotificationRuleState> _state { get; set; }
    
    [Inject]
    private IState<ApplicationsListState> _applicationState { get; set; }
    
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public long Id { get; set; }
    
    [Parameter]
    public EventCallback<long> OnSaved { get; set; }
    
    public SetRuleRequest Model = new() { Type = NotificationType.Email.ToString() };
    private EditContext _editContext;
    private MyButton _btnSubmit;
    private MyEditForm _editForm;
    private bool _isLoading = false;
    private bool _isShowDeleteModal = false;

    private bool _isNew => Id == 0;
    private bool _canAddCondition => Model.Conditions.Count < 10;

    private ICollection<DropDownListItem> _messageTemplateDownListItems =>
        _state.Value.MessageTemplates.Select(item => new DropDownListItem
        {
            Id = item.Id.ToString(),
            Label = item.Subject.Truncate(20),
            Description = item.Body.Truncate(20)
        }).ToList();
    
    private ICollection<DropDownListItem> _applicationDownListItems =>
        _applicationState.Value.List.Select(item => new DropDownListItem
        {
            Id = item.Id.ToString(),
            Label = item.Name
        }).ToList();

    private ICollection<DropDownListItem> _operatorTypesDownListItems => LogicOperatorType.Conjunction.ToDropDownList();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _editContext = new EditContext(Model);
        Dispatcher.Dispatch(new FetchMessageTemplatesAction(true));
        Dispatcher.Dispatch(new FetchNextListPageAction(true));
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (Model.Id != Id)
        {
            Model.Id = Id;
            if (_isNew)
            {
                Model.Reset();
            }
            else
            {
                var foundItem = _state.Value.Rules.FirstOrDefault(
                    item => item.Id == Id
                );
                Model.Fill(foundItem);
            }
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
        var isValid = _editContext.Validate();
        if (isValid)
        {
            _isLoading = true;
            try
            {
                var item = await _apiService.NotificationRuleSet(Model);
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Info,
                    Summary = _isNew ? NotificationResources.Rules_Added : NotificationResources.Rules_Saved    
                });
                Dispatcher.Dispatch(new SetNotificationRuleAction(item));
                await InvokeAsync(async () =>
                {
                    await OnSaved.InvokeAsync(item.Id);
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
            finally
            {
                _isLoading = false;
            }
            StateHasChanged();
        }
    }

    private void OnAddAction()
    {
        _editForm.ClickAsync().Wait();
    }
    
    private void OnAddCondition()
    {
        Model.Conditions.Add(new SetConditionRequest());
    }
    
    private void OnDeleteCondition(SetConditionRequest condition)
    {
        Model.Conditions.Remove(condition);
    }

    private async Task OnDeleteRuleAsync()
    {
        _isShowDeleteModal = false;
        _isLoading = true;
        try
        {
            var id = Model.Id.Value;
            await _apiService.NotificationRuleDelete(id);
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = _isNew ? NotificationResources.Rules_Added : NotificationResources.Rules_Saved    
            });
            Dispatcher.Dispatch(new DeleteNotificationRuleAction(id));
            await InvokeAsync(async () =>
            {
                await OnSaved.InvokeAsync(0);
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
        finally
        {
            _isLoading = false;
        }
        StateHasChanged();
    }
}

