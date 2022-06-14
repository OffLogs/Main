using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Constants;
using OffLogs.Web.Services.i18;
using OffLogs.Web.Store.Auth;
using Radzen.Blazor;

namespace OffLogs.Web.Shared;

public partial class MainHeader
{
    [Inject]
    private ILocalizationService LocalizationService { get; set; }
    
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    
    [Inject]
    private IState<AuthState> AuthState { get; set; }

    private ICollection<CultureInfo> _languageMenuItems;
    private string _currentLocaleId;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _languageMenuItems = LocalizationService.GetAwailableLocales();
        _currentLocaleId = LocalizationService.GetLocale().Name;
    }
    
    private async Task OnSelectLanguageAsync(string id)
    {
        await LocalizationService.SetLocaleAsync(id);
        NavigationManager.NavigateTo("/", forceLoad: true);
    }
    
    private void OnLogout()
    {
        Dispatcher.Dispatch(new LogoutAction());
        NavigationManager.NavigateTo("/", true);
    }
    
    private void OnClickUserMenu(RadzenProfileMenuItem menuEvent)
    {
        switch (menuEvent.Value)
        {
            case "logout":
                Dispatcher.Dispatch(new LogoutAction());
                NavigationManager.NavigateTo("/", true);
                break;
        }
    }
}
