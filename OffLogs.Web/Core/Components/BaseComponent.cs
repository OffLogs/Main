using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OffLogs.Web.Services;
using Radzen;

namespace OffLogs.Web.Core.Components;

public class BaseComponent: Fluxor.Blazor.Web.Components.FluxorComponent
{
    [Parameter]
    public string? Locale { get; set; }
    
    [Inject]
    protected IDispatcher Dispatcher { get; set; }
    
    [Inject] 
    protected NotificationService NotificationService { get; set; }
    
    [Inject]
    protected IJSRuntime Js { get; set; }
}
