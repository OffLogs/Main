using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Services;

namespace OffLogs.Web.Core.Components;

public class BaseComponent: Fluxor.Blazor.Web.Components.FluxorComponent
{
    [Parameter]
    public string? Locale { get; set; }
    
    [Inject]
    protected IDispatcher Dispatcher { get; set; }
    
    [Inject]
    protected ToastService ToastService { get; set; }
}
