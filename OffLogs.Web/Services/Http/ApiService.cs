using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Common.Models.Http;
using OffLogs.Web.Core.Constants;
using OffLogs.Web.Core.Exceptions;

namespace OffLogs.Web.Services.Http
{
    public class ApiService: IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ApiService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task<JsonCommonResponse<T>> RequestAsync<T>(string requestUri, string jwtToken, object data, HttpMethod httpMethod)
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
                return JsonConvert.DeserializeObject<JsonCommonResponse<T>>(responseString);
            }

            throw new HttpResponseStatusException(response.StatusCode, "Add message");
        }

        private async Task<JsonCommonResponse<T>> PostAsync<T>(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync<T>(requestUri, jwtToken, data, HttpMethod.Post);
        }    
        
        private async Task<JsonCommonResponse<T>> PostAuthorizedAsync<T>(string requestUri, object data)
        {
            return await RequestAsync<T>(
                requestUri, 
                await _localStorage.GetItemAsync<string>(AuthorizationService.AuthKey), 
                data, 
                HttpMethod.Post
            );
        }
        
        private async Task<JsonCommonResponse<T>> GetAsync<T>(string requestUri, object data, string jwtToken = null)
        {
            return await RequestAsync<T>(requestUri, jwtToken, data, HttpMethod.Get);
        }  
        
        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            var response = await PostAsync<LoginResponseModel>(ApiUrl.Login, model);
            if (response == null)
            {
                throw new ServerErrorException();
            }

            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }

            return response?.Data;
        }
        
        public async Task<bool> CheckIsLoggedInAsync(string token)
        {
            var response = await GetAsync<object>(ApiUrl.UserCheckIsLoggedIn, null, token);
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response.IsSuccess;
        }
        
        public async Task<PaginatedResponseModel<ApplicationResponseModel>> GetApplications(PaginatedRequestModel request)
        {
            var response = await PostAuthorizedAsync<PaginatedResponseModel<ApplicationResponseModel>>(ApiUrl.ApplicationList, request);
            if (response == null)
            {
                throw new ServerErrorException();
            }

            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }

            return response?.Data;
        }
    }
}