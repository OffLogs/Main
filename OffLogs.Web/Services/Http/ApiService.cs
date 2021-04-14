using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Common.Models.Http;
using OffLogs.Web.Core.Constants;
using OffLogs.Web.Core.Exceptions;

namespace OffLogs.Web.Services.Http
{
    public class ApiService: IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<JsonCommonResponse<T>> PostRequestAsync<T>(string requestUri, object data = null)
        {
            // create request object
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = JsonContent.Create(data);
            // add authorization header
            // request.Headers.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            // send request
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<JsonCommonResponse<T>>(responseString);
            }

            throw new HttpResponseStatusException(response.StatusCode, "Add message");
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            var response = await PostRequestAsync<LoginResponseModel>(ApiUrl.Login, model);
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