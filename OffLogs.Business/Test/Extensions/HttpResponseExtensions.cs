using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Vizit.Business.Models.Http;

namespace OffLogs.Business.Test.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task<JsonCommonResponse<T>> GetJsonDataAsync<T>(this HttpResponseMessage response)
        {
            var stringData = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JsonCommonResponse<T>>(stringData);
        }
    }
}
