using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Services.Validation;

namespace OffLogs.Web.Pages.User.Registration;

public partial class Step1
{
    [Inject] 
    private IApiService _apiService { get; set; }
    
    [Inject] 
    private NavigationManager _navigationManager { get; set; }
    
    [Inject] 
    private ToastService _toastService { get; set; }
    
    [Inject] 
    private IReCaptchaService _reCaptchaService { get; set; }
    
    private RegistrationStep1Request model = new();
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = false;
        await UpdateReCaptchaAsync();
    }

    private async Task UpdateReCaptchaAsync()
    {
        model.ReCaptcha = await _reCaptchaService.GetReCaptchaTokenAsync();
    }
    
    private async Task HandleSubmit()
    {
        _isLoading = true;
        try
        {
            var isOk = await _apiService.RegistrationStep1Async(model);
            if (isOk)
            {
                _toastService.AddInfoMessage(AuthResources.Registration_EmailIsSent);
                model.Email = "";
            }
        }
        catch (Exception)
        {
            _toastService.AddErrorMessage(AuthResources.Registration_RegistrationError);
        }
        finally
        {
            _isLoading = false;
        }
        StateHasChanged();
        await UpdateReCaptchaAsync();
    }
}
