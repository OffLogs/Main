using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;
using System;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Application.Actions
{
    public class GetRequestHandler : IAsyncRequestHandler<GetRequest, ApplicationDto>
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
            _queryBuilder = queryBuilder;
            _applicationService = applicationService;
            _requestService = requestService;
            _accessPolicyService = accessPolicyService;
        }

        public async Task<ApplicationDto> ExecuteAsync(GetRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _accessPolicyService.HasWriteAccessAsync<ApplicationEntity>(request.Id, userId))
            {
                throw new DataPermissionException();
            }
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(request.Id);
            return _mapper.Map<ApplicationDto>(application);
        }
    }
}
