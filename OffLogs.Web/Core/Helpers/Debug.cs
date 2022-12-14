using System;
using Newtonsoft.Json;

namespace OffLogs.Web.Core.Helpers;

public static class Debug
{
    public static void Log(params object[] vals)
    {
        var values = "";
        foreach (var val in vals)
        {
            values += JsonConvert.SerializeObject(val) + " ";    
        }
        Console.WriteLine(values);
    }
}
