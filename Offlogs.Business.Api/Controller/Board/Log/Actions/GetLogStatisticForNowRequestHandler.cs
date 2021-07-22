using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace Offlogs.Business.Api.Controller.Board.Log.Actions
{
    public class GetLogStatisticForNowRequestHandler : IAsyncRequestHandler<GetLogStatisticForNowRequest, LogStatisticForNowDto>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;

        public GetLogStatisticForNowRequestHandler(
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService
        )
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _requestService = requestService;
            _accessPolicyService = accessPolicyService;
        }

        public async Task<LogStatisticForNowDto> ExecuteAsync(GetLogStatisticForNowRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _accessPolicyService.HasWriteAccessAsync<ApplicationEntity>(request.ApplicationId, userId))
            {
                throw new DataPermissionException();
            }
            var statisticList = await _queryBuilder.For<ICollection<OffLogs.Business.Orm.Dto.Entities.LogStatisticForNowDto>>()
                .WithAsync(new LogGetStatisticForNowCriteria(request.ApplicationId));
            return new LogStatisticForNowDto(statisticList);
        }
    }
}
