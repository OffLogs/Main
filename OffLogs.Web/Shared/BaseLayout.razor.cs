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

public partial class BaseLayout
{
    [Inject]
    protected IAuthorizationService AuthService { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    [Inject]
    protected ToastService ToastService { get; set; }
    
    [Inject]
    protected IGlobalEventsService EventsService { get; set; }
    
    [Inject]
    protected IReCaptchaService ReCaptchaService { get; set; }
    
    [Inject]
    protected IState<ToastMessagesState> ToastMessagesState { get; set; }
    
    [Inject]
    protected IState<AuthState> AuthState { get; set; }
    
    [Inject]
    protected IState<CommonState> CommonState { get; set; }
    
    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    protected bool IsRedirectIfNotLoggedIn = true;

    protected bool _isShowMainMenu
    {
        get => AuthState.Value.IsLoggedIn && NavigationManager.GetPath().ToLower().StartsWith(SiteUrl.Dashboard);
    }

    protected bool _isShowReCaptcha = false;

    private bool _isSharedPage
    {
        get
        {
            var path = NavigationManager.GetPath();
            return path.Equals("/") 
                   || path.StartsWith("/login")
                   || path.StartsWith("/registration")
                   || path.StartsWith("/documentation");
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        CommonState.StateChanged += async (sender, args) =>
        {
            if (CommonState.Value.IsInitialized)
            {
                await InitAppAsync();
            }
        };
        Dispatcher.Dispatch(new LoadPersistedDataAction());
    }

    private async Task InitAppAsync()
    {
        NavigationManager.LocationChanged += (sender, args) =>
        {
            CheckIsLoggedInAndRedirect();
        };
        AuthState.StateChanged += (sender, args) =>
        {
            CheckIsLoggedInAndRedirect();
        };
        ReCaptchaService.IsShowChanged += OnReCaptchaShowChanged;
        _isShowReCaptcha = ReCaptchaService.GetIsEnabled();
        
        await AuthService.CheckIsLoggedInAsync();
        CheckIsLoggedInAndRedirect();
    }

    public void Dispose()
    {
        ReCaptchaService.IsShowChanged -= OnReCaptchaShowChanged;
    }

    private void OnReCaptchaShowChanged(bool isShow)
    {
        _isShowReCaptcha = isShow;
    }
    
    private void CheckIsLoggedInAndRedirect()
    {
        if (!IsRedirectIfNotLoggedIn)
        {
            return;
        }

        if (!AuthState.Value.IsLoggedIn && !_isSharedPage)
        {
            NavigationManager.NavigateTo("/login");
        }
        StateHasChanged();
    }
}
