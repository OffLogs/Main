using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using OffLogs.Web.Resources;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;

namespace OffLogs.Web.Pages.Dashboard;

public partial class Index
{
    private readonly ICollection<MenuItem> _menuItems = new List<MenuItem>();

    private ICollection<long> items = new List<long>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _menuItems.Add(
            new MenuItem()
            {
                Id = "statistic",
                Title = CommonResources.DashboardMenu_Statistic
            }
        );
    }
}
