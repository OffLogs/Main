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
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
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
            var response = await GetAsync(MainApiUrl.UserCheckIsLoggedIn, null, token);
            if (string.IsNullOrEmpty(response))
            {
                throw new ServerErrorException();
            }
            return true;
        }
    }
}
