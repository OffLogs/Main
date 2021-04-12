using System.Net.Http;
using System.Threading.Tasks;

namespace OffLogs.Web.Services.Http
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        // public async Task<>
    }
}