using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace OffLogs.Web.Core.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static string GetPath(this NavigationManager manager)
        {
            return new Uri(manager.Uri).AbsolutePath;
        }
        
        public static string GetPath(this LocationChangedEventArgs manager)
        {
            return new Uri(manager.Location).AbsolutePath;
        }
    }
}
