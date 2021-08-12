using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Api.Common.Requests.Board.Log;
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
        public async Task<UsersListDto> FindUsers(string search)
        {
            var response = await PostAuthorizedAsync<UsersListDto>(MainApiUrl.UserSearch, new SearchRequest { 
                Search = search
            });
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
    }
}
