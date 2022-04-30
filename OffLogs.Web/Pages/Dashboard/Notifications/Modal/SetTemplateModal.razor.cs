using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Models.Modal;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;
using OffLogs.Web.Store.Notification;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Modal;

public partial class SetTemplateModal
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
    public EventCallback OnClose { get; set; }
    
    [Parameter]
    public bool IsShowAddModal { get; set; } = false;
    
    [Parameter]
    public long Id { get; set; }
    
    private SetMessageTemplateRequest _model = new();
    private EditContext _editContext;
    private MyButton _btnSubmit;
    private MyEditForm _editForm;
    private bool _isLoading = false;
    

    private List<ModalButtonModel> _modalButtons = new ();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _editContext = new EditContext(_model);

        var saveBtnModel = new ModalButtonModel(
            "Add",
            OnAddAction
        )
        {
            IsCloseAfterAction = false
        };
        _modalButtons.Add(saveBtnModel);
        _modalButtons.Add(new ModalButtonModel(
            "Cancel",
            OnCloseAction,
            BootstrapColorType.Light
        ));
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (_model.Id != Id)
        {
            var foundItem = _state.Value.MessageTemplates.FirstOrDefault(
                item => item.Id == Id
            );
            _model.Fill(foundItem);
        }
    }
    
    private async Task HandleSubmit()
    {
        var isValid = _editContext.Validate();
        if (isValid)
        {
            _isLoading = true;
            try
            {
                var item = await _apiService.MessageTemplateSet(_model);
                Dispatcher.Dispatch(new SetMessageTemplatesAction(item));
                OnCloseAction();
                IsShowAddModal = false;
                _toastService.AddInfoMessage(NotificationResources.MessageTemplate_Added);
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
    
    private void OnCloseAction()
    {
        InvokeAsync(async () => await OnClose.InvokeAsync());
    }    
}

