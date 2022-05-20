using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Web.Constants;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Validation;
using Radzen;

namespace OffLogs.Web.Pages.User;

public partial class Login
{
    [Inject] 
    private IAuthorizationService _authorizationService { get; set; }
    
    [Inject] 
    private NavigationManager _navigationManager { get; set; }
    
    [Inject] 
    private NotificationService _notificationService { get; set; }
    
    [Inject] 
    private ILogger<Login> _logger { get; set; }
    
    [Inject] 
    private IReCaptchaService _reCaptchaService { get; set; }
    
    private LoginRequest model = new();
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _isLoading = false;
        await UpdateReCaptchaAsync();
    }

    private async Task UpdateReCaptchaAsync()
    {
        model.ReCaptcha = await _reCaptchaService.GetReCaptchaTokenAsync();
    }

    private async Task OnSelectFiles(InputFileChangeEventArgs changeEvent)
    {
        _isLoading = true;
        var keyFile = changeEvent.GetMultipleFiles(1).FirstOrDefault();
        try
        {
            if (keyFile == null)
            {
                throw new Exception();
            }

            await using var stream = keyFile.OpenReadStream(5000000);
            using var reader = new StreamReader(stream);
            model.Pem = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty(model.Pem))
            {
                throw new Exception("Empty key file");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            _notificationService.Notify(NotificationSeverity.Error, AuthResources.Login_PleaseEnterCorrectKeyFile);
        }
        _isLoading = false;
        await UpdateReCaptchaAsync();
    }

    private async Task HandleSubmit()
    {
        _isLoading = true;
        try
        {
            var isLoggedIn = await _authorizationService.LoginAsync(model);
            if (isLoggedIn)
            {
                _navigationManager.NavigateTo(SiteUrl.Dashboard);
                return;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            _notificationService.Notify(NotificationSeverity.Error, AuthResources.Login_SecretOrPasswordIsIncorrect);
        }
        finally
        {
            _isLoading = false;
        }
        StateHasChanged();
    }
}
