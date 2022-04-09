using System;
using System.Globalization;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using OffLogs.Web.Core.Helpers;

namespace OffLogs.Web.Services.i18;

public class LocalizationService: ILocalizationService
{
    private const string LocalStorageKey = "OffLogs.Locale";
    private static string _defaultLocale = "en-US";
    
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<LocalizationService> _logger;

    public LocalizationService(
        ILocalStorageService localStorage,
        ILogger<LocalizationService> logger
    )
    {
        _localStorage = localStorage;
        _logger = logger;
    }
    
    public async Task PreConfigureFromLocalStorageAsync()
    {
        var locale = await _localStorage.GetItemAsStringAsync(LocalStorageKey) ?? _defaultLocale;
        await SetLocaleAsync(locale);
    }

    public async Task SetLocaleAsync(string locale)
    {
        var culture = new CultureInfo(locale);
        await _localStorage.SetItemAsStringAsync(LocalStorageKey, locale);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        _logger.LogDebug($"Selected locale: {locale}");
    }
}
