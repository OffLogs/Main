using System;
using System.Threading.Tasks;

namespace OffLogs.Web.Pages.Dashboard.Shared;

public partial class Layout: IDisposable
{
    protected override async Task OnInitializedAsync()
    {
        IsRedirectIfNotLoggedIn = true;
        await base.OnInitializedAsync();
    }
}
