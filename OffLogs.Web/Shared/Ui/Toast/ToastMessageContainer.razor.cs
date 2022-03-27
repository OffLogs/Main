using System.Collections.Generic;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Core.Models.Toast;
using OffLogs.Web.Services;
using OffLogs.Web.Store.Shared.Toast;

namespace OffLogs.Web.Shared.Ui.Toast;

public partial class ToastMessageContainer
{
    [Inject]
    private ToastService _toastService { get; set; }

    [Inject]
    private IState<ToastMessagesState> _state { get; set; }
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _toastService.Start();
    }
}
