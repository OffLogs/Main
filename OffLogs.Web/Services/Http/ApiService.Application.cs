using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Common.Models.Http;
using OffLogs.Web.Core.Exceptions;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {

        public async Task<PaginatedResponseModel<ApplicationResponseModel>> GetApplications(PaginatedRequestModel request = null)
        {
            if (request == null)
            {
                request = new PaginatedRequestModel()
                {
                    Page = 1
                };
            }
            var response = await PostAuthorizedAsync<PaginatedResponseModel<ApplicationResponseModel>>(MainApiUrl.ApplicationList, request);
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

        public async Task<ApplicationResponseModel> GetApplication(long logId)
        {
            var response = await PostAuthorizedAsync<ApplicationResponseModel>(
                MainApiUrl.ApplicationGetOne, 
                new LogGetOneRequestModel() { 
                    Id = logId    
                }
            );
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