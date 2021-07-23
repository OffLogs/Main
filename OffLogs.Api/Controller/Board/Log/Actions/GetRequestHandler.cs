using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class GetRequestHandler : IAsyncRequestHandler<GetRequest, LogDto>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;

        public GetRequestHandler(
            IJwtAuthService jwtAuthService,
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService
        )
        {
            _jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
        }

        public async Task<LogDto> ExecuteAsync(GetRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(request.Id);
            if (log == null)
            {
                throw new ItemNotFoundException(nameof(log));
            }
            if (!await _accessPolicyService.HasReadAccessAsync<ApplicationEntity>(log.Application.Id, userId))
            {
                throw new DataPermissionException();
            }

            return _mapper.Map<LogDto>(log);
        }
    }
}
