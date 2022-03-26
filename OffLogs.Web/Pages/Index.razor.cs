using Fluxor;
using Microsoft.AspNetCore.Components;

namespace OffLogs.Web.Pages;

public partial class Index
{
    [Inject]
    public IDispatcher Dispatcher { get; set; }
}
