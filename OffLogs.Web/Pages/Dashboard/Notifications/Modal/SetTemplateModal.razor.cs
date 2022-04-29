using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Models.Modal;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui;
using OffLogs.Web.Shared.Ui.Form;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Modal;

public partial class SetTemplateModal
{
    [Inject]
    private IApiService _apiService { get; set; }

    [Inject]
    private ToastService _toastService { get; set; }
    
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<ApplicationDto> OnAdded { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }
    
    [Parameter]
    public bool IsShowAddModal { get; set; } = false;
    
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
                var application = await _apiService.AddApplicationAsync(_model.Subject);
                await OnAdded.InvokeAsync(application);
                OnCloseAction();
                IsShowAddModal = false;
                _toastService.AddInfoMessage("New application was added");
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

