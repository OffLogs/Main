using System;

namespace OffLogs.Web.Core.Models.Modal
{
    public class ModalButtonModel
    {
        public string Type { get; set; } = "primary";

        public string Name { get; set; }

        public event Action OnClick;

        public ModalButtonModel(string name, Action onClick, string type = "primary")
        {
            Type = type;
            Name = name;
            OnClick = onClick;
        }
    }
}