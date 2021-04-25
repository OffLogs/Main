using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Web.Core.Exceptions;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        public const string AuthKey = "OffLogs_JwtToken";
        
        private readonly IApiService _apiService;
        private readonly ILocalStorageService _localStorage;

        private bool _isLoggedIn = false;

        public AuthorizationService(IApiService apiService, ILocalStorageService localStorage)
        {
            _apiService = apiService;
            _localStorage = localStorage;
        }

        public bool IsLoggedIn()
        {
            return _isLoggedIn;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(AuthKey);
        }

        public async Task<bool> LoginAsync(LoginRequestModel model)
        {
            var loginData = await _apiService.LoginAsync(model);
            var jwtToken = loginData?.Token;
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _isLoggedIn = true;
                await _localStorage.SetItemAsync(AuthKey, jwtToken);
                return true;
            }
            return false;
        }
        
        public async Task<bool> IsHasJwtAsync()
        {
            return !string.IsNullOrEmpty(await GetJwtAsync());
        }
        
        public async Task<bool> CheckIsLoggedInAsync()
        {
            if (await IsHasJwtAsync())
            {
                _isLoggedIn = await _apiService.CheckIsLoggedInAsync(await GetJwtAsync());
            }
            else
            {
                _isLoggedIn = false;    
            }
            return _isLoggedIn;
        }

        public async Task<string> GetJwtAsync()
        {
            return await _localStorage.GetItemAsStringAsync(AuthKey);
        }
    }
}