﻿using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
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
        public async Task<PaginatedListDto<LogListItemDto>> GetLogs(GetListRequest request)
        {
            var response = await PostAuthorizedAsync<PaginatedListDto<LogListItemDto>>(MainApiUrl.LogList, request);
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

        public async Task<bool> LogSetIsFavorite(long logId, bool isFavorite)
        {
            var response = await PostAuthorizedAsync<bool>(
                MainApiUrl.LogSetIsFavorite,
                new SetIsFavoriteRequest()
                {
                    LogId = logId,
                    IsFavorite = isFavorite
                }
            );
            return response;
        }

        public async Task<LogStatisticForNowDto> LogGetStatisticForNow(long? applicationId = null)
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