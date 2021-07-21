using Api.Requests.Abstractions;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Entities.Application;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Business.Services.Api;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class GetLogStatisticForNowRequestHandler : IAsyncRequestHandler<GetLogStatisticForNowRequest, LogStatisticForNowDto>
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

        public async Task<LogStatisticForNowDto> ExecuteAsync(GetLogStatisticForNowRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _applicationService.IsOwner(userId, request.ApplicationId))
            {
                throw new DataPermissionException();
            }
            var statisticList = await _queryBuilder.For<ICollection<OffLogs.Business.Orm.Dto.Entities.LogStatisticForNowDto>>()
                .WithAsync(new LogGetStatisticForNowCriteria(request.ApplicationId));
            return new LogStatisticForNowDto(statisticList);
        }
    }
}
