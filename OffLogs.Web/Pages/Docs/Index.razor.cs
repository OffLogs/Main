using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Web.Resources;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;

namespace OffLogs.Web.Pages.Docs;

public partial class Index
{
    private readonly ICollection<MenuItem> _menuItems = new List<MenuItem>();
    
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
