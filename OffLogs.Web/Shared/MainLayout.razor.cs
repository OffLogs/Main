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

namespace OffLogs.Web.Shared;

public partial class MainLayout: IDisposable
{
    protected override async Task OnInitializedAsync()
    {
        IsRedirectIfNotLoggedIn = true;
        await base.OnInitializedAsync();
    }

    private void OnClickDocument()
    {
        EventsService.InvokeOnClickDocumentAsync();
    }
}
