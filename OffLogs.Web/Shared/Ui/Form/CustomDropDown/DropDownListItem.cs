namespace OffLogs.Web.Shared.Ui.Form.CustomDropDown
{
    public record DropDownListItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }
}