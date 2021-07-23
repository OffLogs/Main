using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Web.Core.Exceptions;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService: IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ApiService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task<TResponse> RequestAsync<TResponse>(string requestUri, string jwtToken, object data, HttpMethod httpMethod)
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
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(responseString);
            }

            throw new HttpResponseStatusException(response.StatusCode, "Add message");
        }

        private async Task<TResponse> PostAsync<TResponse>(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync<TResponse>(requestUri, jwtToken, data, HttpMethod.Post);
        }    
        
        private async Task<TResponse> PostAuthorizedAsync<TResponse>(string requestUri, object data)
        {
            return await RequestAsync<TResponse>(
                requestUri, 
                await _localStorage.GetItemAsync<string>(AuthorizationService.AuthKey), 
                data, 
                HttpMethod.Post
            );
        }
        
        private async Task<TResponse> GetAsync<TResponse>(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync<TResponse>(requestUri, jwtToken, data, HttpMethod.Get);
        }  
        
        public async Task<LoginResponseDto> LoginAsync(LoginRequest model)
        {
            var response = await PostAsync<LoginResponseDto>(MainApiUrl.Login, model);
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response;
        }
        
        public async Task<bool> CheckIsLoggedInAsync(string token)
        {
            var response = await GetAsync<object>(MainApiUrl.UserCheckIsLoggedIn, null, token);
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return true;
        }
    }
}