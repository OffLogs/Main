using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class SetIsFavoriteRequestHandler : IAsyncRequestHandler<SetIsFavoriteRequest>
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly IApplicationService _applicationService;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;

        public SetIsFavoriteRequestHandler(
            IMapper mapper,
            ILogService logService,
            IApplicationService applicationService,
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
        }

        public async Task ExecuteAsync(SetIsFavoriteRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(request.LogId);
            if (log == null)
            {
                throw new ItemNotFoundException(nameof(log));
            }
            if (!await _accessPolicyService.HasWriteAccessAsync<ApplicationEntity>(log.Application.Id, userId))
            {
                throw new DataPermissionException();
            }
            await _logService.SetIsFavoriteAsync(request.LogId, request.IsFavorite);
        }
    }
}
