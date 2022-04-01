using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace QRID.Mail.UI.Shared.Layout.Navigation.Models
{
    public class HeaderMenuButton
    {
        private static int _id = 0;
        public int Id { get; }
        public string Title { get; }
        public string Icon { get; }
        public ICollection<HeaderMenuButton> PopupItems { get; }
        public EventCallback? OnClick;

        public HeaderMenuButton(
            string title, 
            string icon = "", 
            Action onClick = null, 
            ICollection<HeaderMenuButton> popupItems = null
        )
        {
            Id = ++_id;
            Title = title;
            Icon = icon;
            PopupItems = popupItems == null ? new List<HeaderMenuButton>() : popupItems;
            if (onClick != null)
            {
                OnClick = new EventCallbackFactory().Create(this, onClick);
            }
        }
    }
}
