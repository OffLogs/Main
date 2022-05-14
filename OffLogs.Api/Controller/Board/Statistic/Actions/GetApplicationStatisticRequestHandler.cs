using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Statistic;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Statistic.Actions
{
    public class GetApplicationStatisticRequestHandler : IAsyncRequestHandler<GetApplicationStatisticRequest, ApplicationStatisticDto>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;
        private readonly IMapper _mapper;

        public GetApplicationStatisticRequestHandler(
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService,
            IMapper mapper
        )
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService;
            _accessPolicyService = accessPolicyService;
            _mapper = mapper;
        }

        public async Task<ApplicationStatisticDto> ExecuteAsync(GetApplicationStatisticRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _accessPolicyService.HasReadAccessAsync<ApplicationEntity>(request.ApplicationId, userId))
            {
                throw new DataPermissionException();
            }
            var statistic = await _queryBuilder.For<OffLogs.Business.Orm.Dto.Entities.ApplicationStatisticDto>()
                .WithAsync(new FindByIdCriteria(request.ApplicationId));
            return _mapper.Map<ApplicationStatisticDto>(statistic);
        }
    }
}
