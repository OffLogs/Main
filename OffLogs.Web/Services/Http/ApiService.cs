using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Newtonsoft.Json;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Web.Store.Auth;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Web.Core.Helpers;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService: IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IServiceProvider _serviceProvider;

        public ApiService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IServiceProvider serviceProvider
        )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _serviceProvider = serviceProvider;
        }

        public string GetJwt()
        {
            var store = _serviceProvider.GetService<IState<AuthState>>();
            return store?.Value.Jwt;
        }
        
        private async Task<string> RequestAsync(string requestUri, string jwtToken, object data, HttpMethod httpMethod)
        {
            // create request object
            var request = new HttpRequestMessage(httpMethod, requestUri);
            if (data != null)
            {
                request.Content = JsonContent.Create(data);    
            }
            // add authorization header
            if (!string.IsNullOrEmpty(jwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }

            // send request
            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString;
            }

            BadResponseDto badResponse = null;
            try
            {
                badResponse = JsonConvert.DeserializeObject<BadResponseDto>(responseString);
            }
            finally
            {
                throw new HttpResponseStatusException(response.StatusCode, badResponse?.Message ?? "Server error");
            }
        }
        
        private async Task<TResponse> RequestAsync<TResponse>(string requestUri, string jwtToken, object data, HttpMethod httpMethod)
        {
            var responseString = await RequestAsync(requestUri, jwtToken, data, httpMethod);
            return JsonConvert.DeserializeObject<TResponse>(responseString);
        }

        private async Task<TResponse> PostAsync<TResponse>(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync<TResponse>(requestUri, jwtToken, data, HttpMethod.Post);
        }    
        
        private async Task<TResponse> PostAuthorizedAsync<TResponse>(string requestUri, object data)
        {
            return await RequestAsync<TResponse>(
                requestUri, 
                GetJwt(), 
                data, 
                HttpMethod.Post
            );
        }
        
        private async Task PostAuthorizedAsync(string requestUri, object data)
        {
            await RequestAsync(
                requestUri, 
                await _localStorage.GetItemAsync<string>(GetJwt()), 
                data, 
                HttpMethod.Post
            );
        }
        
        private async Task<TResponse> GetAsync<TResponse>(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync<TResponse>(requestUri, jwtToken, data, HttpMethod.Get);
        }
        
        private async Task<string> GetAsync(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync(requestUri, jwtToken, data, HttpMethod.Get);
        }
    }
}
