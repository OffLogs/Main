using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

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
        
        public static Dictionary<string, StringValues> GetQueryParameters(this NavigationManager manager)
        {
            var uri = manager.ToAbsoluteUri(manager.Uri);
            return QueryHelpers.ParseQuery(uri.Query);
        }
        
        public static List<string> GetQueryParameterValue(this NavigationManager manager, string name)
        {
            var parameters = manager.GetQueryParameters();
            if (parameters.TryGetValue(name, out var values))
            {
                return values.ToList();
            }
            return new List<string>();
        }
    }
}
