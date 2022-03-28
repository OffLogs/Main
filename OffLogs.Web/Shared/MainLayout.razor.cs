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
    [Inject]
    private IAuthorizationService AuthService { get; set; }
    
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    
    [Inject]
    private ToastService ToastService { get; set; }
    
    [Inject]
    private IGlobalEventsService EventsService { get; set; }
    
    [Inject]
    private IReCaptchaService ReCaptchaService { get; set; }
    
    [Inject]
    private IState<ToastMessagesState> ToastMessagesState { get; set; }
    
    [Inject]
    private IState<AuthState> AuthState { get; set; }
    
    [Inject]
    private IState<CommonState> CommonState { get; set; }
    
    [Inject]
    private IDispatcher Dispatcher { get; set; }

    private bool _isInitialized = false;

    private bool _isLoggedIn = false;

    private bool _isShowMainMenu = false;
    
    private bool _isShowReCaptcha = false;

    private bool _isSharedPage
    {
        get
        {
            var path = NavigationManager.GetPath();
            return path.Equals("/") || path.StartsWith("/login") || path.StartsWith("/registration");
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
        _isInitialized = true;
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
        _isLoggedIn = AuthState.Value.IsLoggedIn;
        if (!_isLoggedIn && !_isSharedPage)
        {
            NavigationManager.NavigateTo("/login");
        }
        _isShowMainMenu = _isLoggedIn && NavigationManager.GetPath().StartsWith(SiteUrl.Dashboard);
        StateHasChanged();
    }

    private async Task OnLogout()
    {
        await AuthService.LogoutAsync();
        CheckIsLoggedInAndRedirect();
        NavigationManager.NavigateTo("/");
    }

    private void OnClickDocument()
    {
        EventsService.InvokeOnClickDocumentAsync();
    }
}
