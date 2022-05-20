using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Pages.User.Registration;

public partial class Step2
{
    [Parameter]
    public string? VerificationToken
    {
        get => model.Token;
        set => model.Token = value;
    }
    
    [Inject] 
    private IApiService _apiService { get; set; }
    
    [Inject] 
    private NavigationManager _navigationManager { get; set; }
    
    [Inject] 
    private ToastService _toastService { get; set; }
    
    [Inject] 
    private IAuthorizationService _authorizationService { get; set; }
    
    private RegistrationStep2Request model = new();
    private EditContext _editContext;
    private bool _isLoading;
    private bool _isSecretFileSaved = false;
    private RegistrationStep2ResponseDto _registrationResponse = null;
    private bool _isRegistered => _registrationResponse != null;

    protected override void OnInitialized()
    {
        _isLoading = false;
    }

    private async Task HandleSubmit()
    {
        _isLoading = true;
        try
        {
            _registrationResponse = await _apiService.RegistrationStep2Async(model);
            if (_registrationResponse != null)
            {
                _toastService.AddInfoMessage(AuthResources.Registration_EmailIsSent);
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
    }
    
    private Task LoginAndContinue()
    {
        if (!_isRegistered)
        {
            return Task.CompletedTask;
        }
        _authorizationService.Login(
            _registrationResponse.JwtToken,
            _registrationResponse.Pem,
            _registrationResponse.PrivateKeyBase64
        );
        _navigationManager.NavigateTo(SiteUrl.Dashboard);
        return Task.CompletedTask;
    }
    
    private async Task SaveFile()
    {
        if (!_isRegistered)
        {
            return;
        }
        _isSecretFileSaved = true;
        var pemBytes = System.Text.Encoding.UTF8.GetBytes(_registrationResponse.Pem);
        await FileHelpers.SaveAsAsync(Js, "offlogs_secrets.pem", pemBytes);
    }
}
