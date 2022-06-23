using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.ApiControllers.Abstractions;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Test.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> GetJsonDataAsync<T>(this HttpResponseMessage response)
        {
            var stringData = await response.GetDataAsStringAsync();
            return JsonHelper.DeserializeObject<T>(stringData);
        }
        
        public static async Task<object> GetJsonDataAsync(this HttpResponseMessage response)
        {
            return await response.GetJsonDataAsync<object>();
        }
        
        public static async Task<BadResponseModel> GetJsonErrorAsync(this HttpResponseMessage response)
        {
            return await response.GetJsonDataAsync<BadResponseModel>();
        }
        
        public static async Task<string> GetDataAsStringAsync(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}
