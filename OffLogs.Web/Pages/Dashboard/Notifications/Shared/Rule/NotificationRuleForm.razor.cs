using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Store.Notification;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Shared.Rule;

public partial class NotificationRuleForm
{
    [Inject]
    private IApiService _apiService { get; set; }

    [Inject]
    private ToastService _toastService { get; set; }
    
    [Inject]
    private IState<NotificationRuleState> _state { get; set; }
    
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public long Id { get; set; }
    
    [Parameter]
    public EventCallback<long> OnSaved { get; set; }
    
    private SetRuleRequest _model = new();
    private EditContext _editContext;
    private MyButton _btnSubmit;
    private MyEditForm _editForm;
    private bool _isLoading = false;
    private bool _isShowDeleteModal = false;

    private bool _isNew => Id == 0;

    private ICollection<DropDownListItem> _messageTemplateDownListItems
    {
        get => _state.Value.MessageTemplates.Select(item => new DropDownListItem
        {
            Id = item.Id.ToString(),
            Label = item.Subject.Truncate(20),
            Description = item.Body.Truncate(20)
        }).ToList();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _editContext = new EditContext(_model);
        Dispatcher.Dispatch(new FetchMessageTemplatesAction(true));
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (_model.Id != Id)
        {
            var foundItem = _state.Value.Rules.FirstOrDefault(
                item => item.Id == Id
            );
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
        var isValid = _editContext.Validate();
        if (isValid)
        {
            _isLoading = true;
            try
            {
                var item = await _apiService.NotificationRuleSet(_model);
                _toastService.AddInfoMessage(
                    _isNew ? NotificationResources.MessageTemplate_Added : NotificationResources.MessageTemplate_Saved    
                );
                // Dispatcher.Dispatch(new SetMessageTemplatesAction(item));
                await InvokeAsync(async () =>
                {
                    await OnSaved.InvokeAsync(item.Id);
                });
            }
            catch (Exception e)
            {
                _toastService.AddErrorMessage(e.Message);
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

    private async Task OnDeleteAppAsync()
    {
        _isShowDeleteModal = false;
        _isLoading = true;
        try
        {
            await _apiService.MessageTemplateDelete(_model.Id.Value);
            _toastService.AddInfoMessage(NotificationResources.MessageTemplate_Deleted);
            Dispatcher.Dispatch(new DeleteMessageTemplatesAction(_model.Id.Value));
            await InvokeAsync(async () =>
            {
                await OnSaved.InvokeAsync(0);
            });
        }
        catch (Exception e)
        {
            _toastService.AddErrorMessage(e.Message);
        }
        finally
        {
            _isLoading = false;
        }
        StateHasChanged();
    }
}

