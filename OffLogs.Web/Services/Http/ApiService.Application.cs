using System;
using System.Threading.Tasks;
using OffLogs.Api.Business.Controller.Board.Application.Actions;
using OffLogs.Api.Business.Dto;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {

        public async Task<PaginatedListDto<ApplicationListItemDto>> GetApplications(GetListRequest request = null)
        {
            if (request == null)
            {
                request = new GetListRequest()
                {
                    Page = 1
                };
            }
            var response = await PostAuthorizedAsync<PaginatedListDto<ApplicationListItemDto>>(
                MainApiUrl.ApplicationList, 
                request
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }

        public async Task<ApplicationDto> GetApplication(long logId)
        {
            var response = await PostAuthorizedAsync<ApplicationDto>(
                MainApiUrl.ApplicationGetOne, 
                new GetRequest() { 
                    Id = logId    
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response;
        }
    }
}