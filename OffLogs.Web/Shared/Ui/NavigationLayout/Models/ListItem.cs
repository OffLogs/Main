using OffLogs.Web.Constants;
using OffLogs.Web.Constants.Bootstrap;

namespace OffLogs.Web.Shared.Ui.NavigationLayout.Models
{
    public class ListItem
    {
        public string Id { get; set; }
        
        public string Title { get; set; }
        
        public string RightTitle { get; set; }
        
        public string SubTitle { get; set; }
        
        public ColorType? TitleColorType { get; set; }

        public long GetIdAsLong()
        {
            long.TryParse(Id, out var result);
            return result;
        }
    }
}
