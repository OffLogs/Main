using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;

namespace OffLogs.Web.Pages.Dashboard.Notifications;

public partial class Index
{
    private ICollection<MenuItem> _menuItems { get; set; } = new List<MenuItem>()
    {
        new() { Id = "rules", Title = NotificationResources.MenuItem_Rules, Icon = "expand" },
        new() { Id = "templates", Title = NotificationResources.MenuItem_MessageTemplates, Icon = "layers" },
    };
    
    private ICollection<ListItem> _listItems = new List<ListItem>();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
    
    private void OnSelectMenuItem(OnSelectEventArgs obj)
    {
        Debug.Log(obj);
    }
}
