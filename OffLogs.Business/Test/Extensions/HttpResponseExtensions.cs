using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using OffLogs.Business.Common.Models.Http;

namespace OffLogs.Business.Test.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> GetJsonDataAsync<T>(this HttpResponseMessage response)
        {
            var stringData = await response.GetJsonDataStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringData);
        }
        
        public static async Task<object> GetJsonDataAsync(this HttpResponseMessage response)
        {
            return await response.GetJsonDataAsync<object>();
        }
        
        public static async Task<string> GetJsonDataStringAsync(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}
