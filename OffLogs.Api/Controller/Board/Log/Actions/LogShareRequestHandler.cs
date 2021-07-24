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
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class LogShareRequestHandler : IAsyncRequestHandler<LogShareRequest, LogShareDto>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;
        private readonly ILogShareService _logShareService;

        public LogShareRequestHandler(
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService,
            ILogShareService logShareService
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
            _logShareService = logShareService ?? throw new ArgumentNullException(nameof(logShareService));
        }

        public async Task<LogShareDto> ExecuteAsync(LogShareRequest request)
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
            var logShare = await _logShareService.Share(log);
            return _mapper.Map<LogShareDto>(logShare);
        }
    }
}
