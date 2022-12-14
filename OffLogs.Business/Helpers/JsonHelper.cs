using System.Text;
using Newtonsoft.Json;

namespace OffLogs.Business.Helpers;

public static class JsonHelper
{
    public static string SerializeToString(object data)
    {
        return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        });
    }
    
    public static byte[] SerializeToBytes(object data)
    {
        var jsonString = SerializeToString(data);
        if (!string.IsNullOrEmpty(jsonString))
        {
            return Encoding.UTF8.GetBytes(jsonString);
        }

        return null;
    }
    
    public static T? DeserializeObject<T>(string value)
    {
        return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        });
    }
}
