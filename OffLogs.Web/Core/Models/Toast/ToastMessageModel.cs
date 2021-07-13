using System;
using OffLogs.Web.Constants;

namespace OffLogs.Web.Core.Models.Toast
{
    public class ToastMessageModel
    {
        private static int _buttonkey = 0;
        
        public int Key { get; }
        public ToastMessageType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ToastMessageModel()
        {
            _buttonkey++;
            Key = _buttonkey;
        }
    }
}