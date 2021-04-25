using System;
using OffLogs.Web.Core.Constants;

namespace OffLogs.Web.Core.Models.Toast
{
    public class ToastMessageModel
    {
        public ToastMessageType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}