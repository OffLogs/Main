using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Events;
using OffLogs.Web.Services.Validation;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Common;
using OffLogs.Web.Store.Common.Actions;
using OffLogs.Web.Store.Shared.Toast;

namespace OffLogs.Web.Pages.Landing.Shared;

public partial class Layout
{
    [Inject]
    private IState<AuthState> AuthState { get; set; }
}
