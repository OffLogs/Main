using System;
using Newtonsoft.Json;

namespace OffLogs.Business.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetAsJson(this Object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}