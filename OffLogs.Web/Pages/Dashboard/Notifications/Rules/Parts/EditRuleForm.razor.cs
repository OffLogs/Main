using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Extensions;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Notification;
using Radzen;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Rules.Parts;

public partial class EditRuleForm
{
    [Inject]
    private IState<NotificationRuleState> _state { get; set; }
    
    [Inject]
    private IState<ApplicationsListState> _applicationState { get; set; }
    
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public long Id { get; set; }

    public SetRuleRequest _model = new() { Type = NotificationType.Email.ToString() };
    private EditContext _editContext;
    private MyButton _btnSubmit;
    private MyEditForm _editForm;
    private bool _isLoading = false;
    private bool _isShowDeleteModal = false;

    private LogicOperatorType _logicOperatorType
    {
        get
        {
            if (string.IsNullOrEmpty(_model.LogicOperator))
            {
                return default;
            }
            return Enum.Parse<LogicOperatorType>(_model.LogicOperator);
        }
        set => _model.LogicOperator = value.ToString();
    }
    
    private long _applicationId
    {
        get => _model.ApplicationId ?? default;
        set => _model.ApplicationId = value;
    }

    private bool _isNew => Id == 0;
    private bool _canAddCondition => _model.Conditions.Count < 10;

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

        _editContext = new EditContext(_model);
        Dispatcher.Dispatch(new FetchMessageTemplatesAction(true));
        Dispatcher.Dispatch(new FetchListPageAction());
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (_model.Id != Id)
        {
            _model.Id = Id;
            if (_isNew)
            {
                _model.Reset();
            }
            else
            {
                var foundItem = _state.Value.Rules.FirstOrDefault(
                    item => item.Id == Id
                );
                _model.Fill(foundItem);
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
                var item = await ApiService.NotificationRuleSet(_model);
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Info,
                    Summary = _isNew ? NotificationResources.Rules_Added : NotificationResources.Rules_Saved    
                });
                Dispatcher.Dispatch(new SetNotificationRuleAction(item));
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
        _model.Conditions.Add(new SetConditionRequest());
    }
    
    private void OnDeleteCondition(SetConditionRequest condition)
    {
        _model.Conditions.Remove(condition);
    }
}
