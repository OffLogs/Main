using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;

namespace OffLogs.Web.Pages.Dashboard.Settings;

public partial class Settings
{
    private ICollection<MenuItem> _menuItems { get; set; } = new List<MenuItem>();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _menuItems.Add(
            new MenuItem()
            {
                Id = "1",
                Title = "Test 1"
            }
        );
    }
}
