using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace Offlogs.Business.Api.Controller.Board.Application.Actions
{
    public class AddRequestHandler : IAsyncRequestHandler<AddRequest, ApplicationDto>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;

        public AddRequestHandler(
            IJwtAuthService jwtAuthService,
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IRequestService requestService
        )
        {
            _jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _requestService = requestService;
        }

        public async Task<ApplicationDto> ExecuteAsync(AddRequest request)
        {
            var userId = _jwtAuthService.GetUserId(_requestService.GetApiToken());
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            var application = await _applicationService.CreateNewApplication(user, request.Name);
            return _mapper.Map<ApplicationDto>(application);
        }
    }
}
