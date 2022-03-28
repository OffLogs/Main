using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Auth.Actions;
using OffLogs.Web.Store.Common.Actions;

namespace OffLogs.Web.Services
{
    public class AuthorizationService: IAuthorizationService
    {   
        private readonly IApiService _apiService;
        private readonly IDispatcher _dispatcher;
        private readonly IServiceProvider _serviceProvider;

        public AuthorizationService(
            IApiService apiService, 
            IDispatcher dispatcher,
            IServiceProvider serviceProvider
        )
        {
            _apiService = apiService;
            _dispatcher = dispatcher;
            _serviceProvider = serviceProvider;
        }

        public async Task LogoutAsync()
        {
            _dispatcher.Dispatch(new LogoutAction());
            _dispatcher.Dispatch(new PersistDataAction());
            await Task.CompletedTask;
        }

        public string GetJwt()
        {
            var store = _serviceProvider.GetService<IState<AuthState>>();
            return store?.Value.Jwt;
        }
        
        public async Task<bool> LoginAsync(LoginRequest model)
        {
            var loginData = await _apiService.LoginAsync(model);
            if (loginData != null)
            {
                Login(loginData?.Token, model.Pem);
                return true;
            }

            return false;
        }
        
        public void Login(string jwtToken, string pem)
        {
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _dispatcher.Dispatch(new LoginAction(pem, jwtToken));
                _dispatcher.Dispatch(new PersistDataAction());
            }
        }
        
        public async Task<bool> IsHasJwtAsync()
        {
            return await Task.FromResult(!string.IsNullOrEmpty(GetJwt()));
        }
        
        public async Task<bool> CheckIsLoggedInAsync()
        {
            bool isValidJwt = false;
            if (await IsHasJwtAsync())
            {
                try
                {
                    isValidJwt = await _apiService.CheckIsLoggedInAsync(GetJwt());
                    if (!isValidJwt)
                    {
                        await LogoutAsync();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(@"CheckIsLoggedIn returned: false");
                }
            }
            return isValidJwt;
        }
    }
}
