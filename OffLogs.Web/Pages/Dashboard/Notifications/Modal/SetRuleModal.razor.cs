using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Models.Modal;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Modal;

public partial class SetRuleModal
{
    [Inject]
    private IApiService _apiService { get; set; }

    [Inject]
    private ToastService _toastService { get; set; }
    
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }
    
    [Parameter]
    public bool IsShowAddModal { get; set; } = false;
    
    private SetRuleRequest _model = new();
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
        );
        saveBtnModel.IsCloseAfterAction = false;
        _modalButtons.Add(saveBtnModel);
        _modalButtons.Add(new (
            "Cancel",
            OnCloseAction,
            BootstrapColorType.Light
        ));
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
                // Dispatcher.Dispatch(new AddMessageTemplatesAction(item));
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

