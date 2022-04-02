using System.Collections.Generic;

namespace OffLogs.Web.Shared.Ui.NavigationLayout.Models
{
    public class MenuItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<ListItem> SubMenuItems { get; set; } = new List<ListItem>();
    }
}
