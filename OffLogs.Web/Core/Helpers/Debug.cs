using System;
using Newtonsoft.Json;

namespace OffLogs.Web.Core.Helpers;

public static class Debug
{
    public static void Log(params object[] vals)
    {
        foreach (var val in vals)
        {
            Console.WriteLine(JsonConvert.SerializeObject(val));    
        }
    }
}
