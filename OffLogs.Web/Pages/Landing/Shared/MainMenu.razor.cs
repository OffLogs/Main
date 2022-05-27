using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Core.Components;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Services.i18;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;
using OffLogs.Web.Store.Auth;

namespace OffLogs.Web.Pages.Landing.Shared;

public partial class MainMenu
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
}
