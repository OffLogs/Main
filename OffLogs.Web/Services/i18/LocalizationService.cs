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
    private static readonly CultureInfo _defaultCulture = new CultureInfo("en-US");
    
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<LocalizationService> _logger;
    
    private CultureInfo _currentCulture;

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
        var locale = await _localStorage.GetItemAsStringAsync(LocalStorageKey) ?? _defaultCulture.ToString();
        await SetLocaleAsync(locale);
    }

    public async Task SetLocaleAsync(string locale)
    {
        _currentCulture = new CultureInfo(locale);
        await _localStorage.SetItemAsStringAsync(LocalStorageKey, locale);
        CultureInfo.DefaultThreadCurrentCulture = _currentCulture;
        CultureInfo.DefaultThreadCurrentUICulture = _currentCulture;
        _logger.LogDebug($"Selected locale: {locale}");
    }
    
    public CultureInfo GetLocale()
    {
        return _currentCulture;
    }

    public string GetLocalePostfix()
    {
        if (_currentCulture == null) throw new ArgumentNullException(nameof(_currentCulture));
        if (
            !string.Equals(
                _currentCulture.ToString(),
                _defaultCulture.ToString(),
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            return _currentCulture.TwoLetterISOLanguageName;
        }

        return "";
    }
}
