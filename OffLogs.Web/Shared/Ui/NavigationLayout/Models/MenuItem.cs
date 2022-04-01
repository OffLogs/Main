using System.Collections.Generic;

namespace OffLogs.Web.Shared.Ui.NavigationLayout.Models
{
    public class MenuItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<SubMenuItem> SubMenuItems { get; set; } = new List<SubMenuItem>();
    }
}
