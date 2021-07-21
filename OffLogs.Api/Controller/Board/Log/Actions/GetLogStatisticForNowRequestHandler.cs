using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Orm.Dto.Entities;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class GetLogStatisticForNowRequestHandler : IAsyncRequestHandler<GetLogStatisticForNowRequest, PaginatedListDto<LogStatisticForNowDto>>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;

        public GetLogStatisticForNowRequestHandler(
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IRequestService requestService
        )
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _requestService = requestService;
        }

        public async Task<PaginatedListDto<LogStatisticForNowDto>> ExecuteAsync(GetLogStatisticForNowRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _applicationService.IsOwner(userId, request.ApplicationId))
            {
                throw new DataPermissionException();
            }
            var statisticList = await _queryBuilder.For<ICollection<LogStatisticForNowDto>>()
                .WithAsync(new LogGetStatisticForNowCriteria(request.ApplicationId));

            return new PaginatedListDto<LogStatisticForNowDto>(statisticList);
        }
    }
}
