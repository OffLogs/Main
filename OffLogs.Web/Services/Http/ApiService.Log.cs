using System;
using System.Threading.Tasks;
using OffLogs.Api.Business.Controller.Board.Log.Actions;
using OffLogs.Api.Business.Dto;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<PaginatedListDto<LogDto>> GetLogs(GetListRequest request)
        {
            var response = await PostAuthorizedAsync<PaginatedListDto<LogDto>>(MainApiUrl.LogList, request);
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }

        public async Task<LogDto> GetLog(long logId)
        {
            var response = await PostAuthorizedAsync<LogDto>(
                MainApiUrl.LogGet, 
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
        
        public async Task<bool> LogSetIsFavorite(long logId, bool isFavorite)
        {
            var response = await PostAuthorizedAsync<bool>(
                MainApiUrl.LogSetIsFavorite, 
                new SetIsFavoriteRequest() { 
                    LogId  = logId,
                    IsFavorite = isFavorite
                }
            );
            return response;
        }
    }
}