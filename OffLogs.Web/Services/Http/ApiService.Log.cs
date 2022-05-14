using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
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
        public async Task<PaginatedListDto<LogListItemDto>> GetLogsAsync(GetListRequest request)
        {
            var response = await PostAuthorizedAsync<PaginatedListDto<LogListItemDto>>(MainApiUrl.LogList, request);
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }

        public async Task<LogDto> GetLogAsync(long logId, string privateKeyBase64)
        {
            var response = await PostAuthorizedAsync<LogDto>(
                MainApiUrl.LogGet,
                new GetRequest()
                {
                    Id = logId,
                    PrivateKeyBase64 = privateKeyBase64
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }

        public async Task LogSetIsFavoriteAsync(long logId, bool isFavorite)
        {
            await PostAuthorizedAsync(
                MainApiUrl.LogSetIsFavorite,
                new SetIsFavoriteRequest()
                {
                    LogId = logId,
                    IsFavorite = isFavorite
                }
            );
        }

        public async Task<LogStatisticForNowDto> LogGetStatisticForNowAsync(long? applicationId = null)
        {
            var response = await PostAuthorizedAsync<LogStatisticForNowDto>(
                MainApiUrl.LogGetStatisticForNow,
                new GetLogStatisticForNowRequest()
                {
                    ApplicationId = applicationId
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
