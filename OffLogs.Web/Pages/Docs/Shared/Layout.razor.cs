using System;
using System.Threading.Tasks;

namespace OffLogs.Web.Pages.Docs.Shared;

public partial class Layout: IDisposable
{
    protected override async Task OnInitializedAsync()
    {
        IsRedirectIfNotLoggedIn = false;
        await base.OnInitializedAsync();
    }
}
