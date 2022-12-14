using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Common.Actions;

namespace OffLogs.Web.Services
{
    public class AuthorizationService: IAuthorizationService
    {   
        private readonly IApiService _apiService;
        private readonly IDispatcher _dispatcher;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(
            IApiService apiService, 
            IDispatcher dispatcher,
            IServiceProvider serviceProvider,
            ILogger<AuthorizationService> logger
        )
        {
            _apiService = apiService;
            _dispatcher = dispatcher;
            _serviceProvider = serviceProvider;
            _logger = logger;
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
            return store?.Value.Jwt?.Trim();
        }
        
        public async Task<bool> LoginAsync(LoginRequest model)
        {
            var loginData = await _apiService.LoginAsync(model);
            if (loginData != null)
            {
                Login(loginData?.Token, model.Pem, loginData.PrivateKeyBase64);
                return true;
            }

            return false;
        }
        
        public void Login(string jwtToken, string pem, string privateKeyBase64)
        {
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _dispatcher.Dispatch(new LoginAction(pem, jwtToken, privateKeyBase64));
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
                catch (Exception e)
                {
                    _logger.LogDebug(@"CheckIsLoggedIn returned: false");
                }
            }
            return isValidJwt;
        }
    }
}
