using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Requests.Board.Log;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class GetLogStatisticForNowRequestHandler : IAsyncRequestHandler<GetLogStatisticForNowRequest, LogStatisticForNowDto>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;
        private readonly IMapper _mapper;

        public GetLogStatisticForNowRequestHandler(
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

        public async Task<LogStatisticForNowDto> ExecuteAsync(GetLogStatisticForNowRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (request.ApplicationId.HasValue)
            {
                if (!await _accessPolicyService.HasReadAccessAsync<ApplicationEntity>(request.ApplicationId.Value, userId))
                {
                    throw new DataPermissionException();
                }
            }
            var statisticList = await _queryBuilder.For<ICollection<OffLogs.Business.Orm.Dto.Entities.LogStatisticForNowDto>>()
                .WithAsync(
                    new GetByApplicationOrUserCriteria(userId, request.ApplicationId)
                );
            return new LogStatisticForNowDto(
                _mapper.Map<ICollection<LogStatisticForNowItemDto>>(statisticList)
            );
        }
    }
}
