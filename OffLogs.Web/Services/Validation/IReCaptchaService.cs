using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace OffLogs.Web.Services.Validation;

public interface IReCaptchaService
{
    public string GetSiteKey();
    
    public string GetScriptUrl();

    public bool GetIsEnabled();

    public Task<string> GetReCaptchaTokenAsync(IJSRuntime js);

    public event Action<bool> IsShowChanged;
}
