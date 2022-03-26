using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using OffLogs.Web.Core.Extensions;

namespace OffLogs.Web.Services.Validation;

public class ReCaptchaService: IReCaptchaService, IDisposable
{
    private readonly NavigationManager _navigationManager;
    public event Action<bool> IsShowChanged;

    private readonly string _siteKey;
    
    private static string _scriptUrlPattern => "https://www.google.com/recaptcha/api.js?render={0}";

    private static List<string> _enabledForRoutes = new()
    {
        "/login",
        "/registration",
    };

    private bool _isEnabled = false;

    private bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            IsShowChanged?.Invoke(_isEnabled);
        }
    }

    public ReCaptchaService(
        IConfiguration configuration,
        NavigationManager navigationManager
    )
    {
        _navigationManager = navigationManager;
        _siteKey = configuration.GetValue<string>("ReCaptcha:SiteKey");
        _navigationManager.LocationChanged += OnLocationChanged;
        IsEnabled = IsEnabledRoute(_navigationManager.GetPath());
    }

    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
    
    public string GetSiteKey()
    {
        return _siteKey;
    }

    public string GetScriptUrl()
    {
        return string.Format(_scriptUrlPattern, _siteKey);
    }

    public bool GetIsEnabled()
    {
        return _isEnabled;
    }
    
    public async Task<string> GetReCaptchaTokenAsync(IJSRuntime js)
    {
        return await js.InvokeAsync<string>(
            "window.getReCaptchaToken",
            _siteKey,
            _siteKey
        );
    }
    
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Console.WriteLine(e.Location);
        IsEnabled = _enabledForRoutes.Any(route => IsEnabledRoute(e.GetPath()));
    }

    private bool IsEnabledRoute(string currentPath)
    {
        return currentPath.Equals("/")
               || currentPath.Equals("/#")
               || _enabledForRoutes.Any(currentPath.StartsWith);
    }
}
