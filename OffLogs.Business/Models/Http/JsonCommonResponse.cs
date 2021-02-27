using Newtonsoft.Json;

namespace Vizit.Business.Models.Http
{
    public class JsonCommonResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; } = "fail";
        
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
    
    public class JsonCommonResponse<T>: JsonCommonResponse
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}