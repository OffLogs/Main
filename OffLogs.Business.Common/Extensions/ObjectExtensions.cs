using System;
using Newtonsoft.Json;

namespace OffLogs.Business.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>  
        ///  Returns true if arrays are equals
        /// </summary>  
        public static T JsonClone<T>(this object oldObject)
        {
            var jsonString = JsonConvert.SerializeObject(oldObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
