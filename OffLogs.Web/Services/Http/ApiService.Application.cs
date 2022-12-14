using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<ApplicationDto> AddApplicationAsync(string name)
        {
            var response = await PostAuthorizedAsync<ApplicationDto>(
                MainApiUrl.ApplicationAdd,
                new AddRequest()
                {
                    Name = name
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response;
        }

        public async Task<ApplicationDto> UpdateApplicationAsync(long id, string name)
        {
            var response = await PostAuthorizedAsync<ApplicationDto>(
                MainApiUrl.ApplicationUpdate,
                new UpdateRequest()
                {
                    Id = id,
                    Name = name
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response;
        }

        public async Task DeleteApplicationAsync(long id)
        {
            await PostAuthorizedAsync(
                MainApiUrl.ApplicationDelete,
                new DeleteRequest()
                {
                    Id = id
                }
            );
        }

        public async Task<PaginatedListDto<ApplicationListItemDto>> GetApplicationsAsync(GetListRequest request = null)
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

        public async Task<ApplicationDto> GetApplicationAsync(long logId)
        {
            var response = await PostAuthorizedAsync<ApplicationDto>(
                MainApiUrl.ApplicationGetOne,
                new GetRequest()
                {
                    Id = logId
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response;
        }

        public async Task<UsersListDto> ApplicationGetSharedUsersAsync(long applicationId)
        {
            var response = await PostAuthorizedAsync<UsersListDto>(
                MainApiUrl.ApplicationGetSharedUser,
                new GetSharedUsersRequest()
                {
                    Id = applicationId
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
