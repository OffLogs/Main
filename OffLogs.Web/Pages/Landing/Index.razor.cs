using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Constants;
using OffLogs.Web.Services;
using OffLogs.Web.Store.Shared.Toast;
using OffLogs.Web.Store.Shared.Toast.Actions;

namespace OffLogs.Web.Pages.Landing;

public partial class Index
{
    [Inject]
    public IDispatcher Dispatcher { get; set; }

    [Inject]
    private ToastService _toastService { get; set; }

    [Inject]
    private IState<ToastMessagesState> _state { get; set; }
    
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; }
}
