using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRID.Mail.UI.Shared.Layout.Navigation.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<SubMenuItem> SubMenuItems { get; set; } = new List<SubMenuItem>();
    }
}
