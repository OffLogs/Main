using System.Net.Http;
using System.Threading.Tasks;

namespace OffLogs.Web.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly HttpClient _httpClient;

        public AuthorizationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool IsLoggedIn()
        {
            return false;
        }
        
        public Task<bool> LoginAsync(string userName, string password)
        {
            // _httpClient.PostAsync()
            return Task.FromResult(true);
        }
        
        public Task<bool> IsLoggedInAsync()
        {
            // _httpClient.PostAsync()
            return Task.FromResult(true);
        }
    }
}