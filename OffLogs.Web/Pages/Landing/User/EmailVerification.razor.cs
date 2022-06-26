using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OffLogs.Web.Constants;

namespace OffLogs.Web.Pages.Landing.User;

public partial class EmailVerification
{
    [Parameter] 
    public string Token { get; set; }

    [Inject] 
    public ILogger<EmailVerification> Logger { get; set; }
    
    private bool _isLoading { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await VerifyAsync();
    }

    private async Task VerifyAsync()
    {
        if (string.IsNullOrEmpty(Token))
        {
            ToErrorPage();
            return;
        }
        try
        {
            await ApiService.UserEmailVerifyAsync(Token);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            ToErrorPage();
        }
        _isLoading = false;
    }

    private void ToErrorPage()
    {
        NavigationManager.NavigateTo(SiteUrl.Error500);
    }
}
