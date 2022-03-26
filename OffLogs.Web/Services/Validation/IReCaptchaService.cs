using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace OffLogs.Web.Services.Validation;

public interface IReCaptchaService
{
    public string GetSiteKey();
    
    public string GetScriptUrl();

    public bool GetIsEnabled();

    public Task<string> GetReCaptchaTokenAsync();

    public event Action<bool> IsShowChanged;
}
