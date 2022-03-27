using Fluxor;
using Microsoft.AspNetCore.Components;

namespace OffLogs.Web.Core.Components;

public class BaseComponent: Fluxor.Blazor.Web.Components.FluxorComponent
{
    [Inject]
    protected IDispatcher Dispatcher { get; set; }
}
