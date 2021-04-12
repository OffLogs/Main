using System;
using Microsoft.AspNetCore.Components;

namespace OffLogs.Web.Core.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static string GetPath(this NavigationManager manager)
        {
            return new Uri(manager.Uri).AbsolutePath;
        }
    }
}