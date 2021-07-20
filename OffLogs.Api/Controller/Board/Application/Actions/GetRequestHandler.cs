using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
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

        public GetRequestHandler(
            IJwtAuthService jwtAuthService,
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService
        )
        {
            this._jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._queryBuilder = queryBuilder;
            this._applicationService = applicationService;
        }

        public async Task<ApplicationDto> ExecuteAsync(GetRequest request)
        {
            var userId = _jwtAuthService.GetUserId();
            if (!await _applicationService.IsOwner(userId, request.Id))
            {
                throw new DataPermissionException();
            }
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(request.Id);
            return _mapper.Map<ApplicationDto>(application);
        }
    }
}
