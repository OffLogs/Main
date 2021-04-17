using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Web.Core.Exceptions;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private const string AuthKey = "OffLogs_JwtToken";
        
        private readonly IApiService _apiService;
        private readonly ILocalStorageService _localStorage;

        public AuthorizationService(IApiService apiService, ILocalStorageService localStorage)
        {
            _apiService = apiService;
            _localStorage = localStorage;
        }

        public bool IsLoggedIn()
        {
            return false;
        }
        
        public async Task<bool> LoginAsync(LoginRequestModel model)
        {
            var loginData = await _apiService.LoginAsync(model);
            var jwtToken = loginData?.Token;
            if (!string.IsNullOrEmpty(jwtToken))
            {
                await _localStorage.SetItemAsync(AuthKey, jwtToken);
                return true;
            }
            return false;
        }
        
        public async Task<bool> IsHasJwtAsync()
        {
            return await _localStorage.ContainKeyAsync(AuthKey);
        }
        
        public Task<bool> CheckIsLoggedIn()
        {
            // _httpClient.PostAsync()
            return Task.FromResult(true);
        }
    }
}