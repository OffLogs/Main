using System.Net.Http;
using System.Threading.Tasks;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly IApiService _apiService;
        
        public AuthorizationService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public bool IsLoggedIn()
        {
            return false;
        }
        
        public async Task<bool> LoginAsync(LoginRequestModel model)
        {
            await _apiService.LoginAsync(model);
            return true;
        }
        
        public Task<bool> IsLoggedInAsync()
        {
            // _httpClient.PostAsync()
            return Task.FromResult(true);
        }
    }
}