using System.Threading.Tasks;

namespace OffLogs.Web.Pages.Landing.Shared;

public partial class Layout
{
    protected override async Task OnInitializedAsync()
    {
        IsRedirectIfNotLoggedIn = false;
        await base.OnInitializedAsync();
    }
}
