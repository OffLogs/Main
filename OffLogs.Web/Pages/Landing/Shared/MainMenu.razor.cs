using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Core.Components;
using OffLogs.Web.Store.Auth;

namespace OffLogs.Web.Pages.Landing.Shared;

public partial class MainMenu
{
    [Inject]
    private IState<AuthState> AuthState { get; set; }
}
