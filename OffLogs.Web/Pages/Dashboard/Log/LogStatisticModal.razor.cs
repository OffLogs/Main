using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Models.Modal;

namespace OffLogs.Web.Pages.Dashboard.Log;

public partial class LogStatisticModal
{
    [Parameter]
    public EventCallback OnClose { get; set; }

    [Parameter]
    public bool IsShowModal { get; set; } = false;
    
    [Parameter]
    public long? ApplicationId { get; set; }
    
    private long? _currentApplicationId { get; set; }
    private List<ModalButtonModel> _modalButtons = new ();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _modalButtons.Add(new (
            "Cancel",
            OnCloseAction,
            BootstrapColorType.Light
        ));
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (ApplicationId == null || !IsShowModal)
        {
            _currentApplicationId = null;
            return;
        }

        if (ApplicationId != _currentApplicationId)
        {
            _currentApplicationId = ApplicationId;    
        }
    }
    
    private void OnCloseAction()
    {
        InvokeAsync(async () => await OnClose.InvokeAsync());
    }
}


