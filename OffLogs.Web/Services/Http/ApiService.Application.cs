using System;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
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
                new ApplicationGetModel() { 
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