using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Core.Components;
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

    private List<DropDownListItem> _languageMenuItems = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _languageMenuItems = LocalizationService.GetAwailableLocales()
            .Select(locale =>
            {
                return new DropDownListItem()
                {
                    Id = locale.Name,
                    Label = locale.DisplayName
                };
            })
            .ToList();
    }
    
    private async Task OnSelectLanguageAsync(DropDownListItem listItem)
    {
        await LocalizationService.SetLocaleAsync(listItem.Id);
        NavigationManager.NavigateTo("/", forceLoad: true);
    }
}
