using OffLogs.Web.Constants;

namespace OffLogs.Web.Shared.Ui.NavigationLayout.Models
{
    public class ListItem
    {
        public string Id { get; set; }
        
        public string Title { get; set; }
        
        public string RightTitle { get; set; }
        
        public string SubTitle { get; set; }
        
        public BootstrapColorType? BgColorType { get; set; }

        public long GetIdAsLong()
        {
            long.TryParse(Id, out var result);
            return result;
        }
    }
}
