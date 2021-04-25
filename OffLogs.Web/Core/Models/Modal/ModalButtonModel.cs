using System;
using Microsoft.AspNetCore.Components;

namespace OffLogs.Web.Core.Models.Modal
{
    public class ModalButtonModel
    {
        private static int _buttonkey = 0;
        
        public bool IsCloseAfterAction { get; set; } = true;
        public string Type { get; set; } = "primary";

        public string Name { get; set; }

        public EventCallback? OnClick;
        
        public int Key { get; }

        public ModalButtonModel()
        {
            _buttonkey++;
            Key = _buttonkey;
            
            Console.WriteLine("Assign key: " + Key);
        }

        public ModalButtonModel(string name, Action? onClick, string type = "primary"): base()
        {
            Type = type;
            Name = name;
            if (onClick != null)
            {
                OnClick = new EventCallbackFactory().Create(this, onClick);
            }
        }
    }
}