using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OffLogs.Web.Core.Helpers;

namespace OffLogs.Web.Services.i18;

public class LocalizationService: ILocalizationService
{
    private const string LocalStorageKey = "OffLogs.Locale";
    private static readonly CultureInfo _defaultLocale = new("en-US");
    private static readonly ICollection<CultureInfo> _allowedCultures = new List<CultureInfo>()
    {
        _defaultLocale,
        new("ru-RU")
    };
    
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<LocalizationService> _logger;
    private readonly NavigationManager _navigationManager;

    private CultureInfo _currentCulture;
    
    public LocalizationService(
        ILocalStorageService localStorage,
        ILogger<LocalizationService> logger,
        NavigationManager navigationManager
    )
    {
        _localStorage = localStorage;
        _logger = logger;
        _navigationManager = navigationManager;
    }
    
    public async Task SetUpLocaleAsync()
    {
        var locale = GetLocaleFromUrl();
        locale ??= await GetLocaleFromStorageAsync();
        locale ??= _defaultLocale;
        await SetLocaleAsync(locale);
    }
    
    public async Task SetLocaleAsync(string locale)
    {
        await SetLocaleAsync(new CultureInfo(locale));
    }

    public async Task SetLocaleAsync(CultureInfo cultureInfo)
    {
        _currentCulture = cultureInfo;
        await _localStorage.SetItemAsStringAsync(LocalStorageKey, cultureInfo.ToString());
        CultureInfo.DefaultThreadCurrentCulture = _currentCulture;
        CultureInfo.DefaultThreadCurrentUICulture = _currentCulture;
        _logger.LogDebug($"Selected locale: {cultureInfo}");
    }

    public CultureInfo GetLocale()
    {
        return _currentCulture;
    }
    
    public ICollection<CultureInfo> GetAwailableLocales()
    {
        return _allowedCultures;
    }

    public string GetLocalePostfix()
    {
        if (_currentCulture == null) throw new ArgumentNullException(nameof(_currentCulture));
        if (
            !string.Equals(
                _currentCulture.ToString(),
                _defaultLocale.ToString(),
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            return _currentCulture.TwoLetterISOLanguageName;
        }

        return "";
    }

    private async Task<CultureInfo> GetLocaleFromStorageAsync()
    {
        var locale = await _localStorage.GetItemAsStringAsync(LocalStorageKey);
        return locale != null ? new CultureInfo(locale) : null;
    }
    
    private CultureInfo GetLocaleFromUrl()
    {
        var urlSegments = new Uri(_navigationManager.Uri).Segments;
        if (urlSegments.Length <= 1)
        {
            return null;
        }
        var cultureFromUrl = urlSegments[1].Replace("/", "").ToLower();
        return _allowedCultures.FirstOrDefault(
            culture => culture.Name.ToLower().StartsWith(cultureFromUrl)
        );
    }
}
