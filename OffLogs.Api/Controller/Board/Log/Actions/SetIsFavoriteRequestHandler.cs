using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Commands.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Commands.Entities.Log;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class SetIsFavoriteRequestHandler : IAsyncRequestHandler<SetIsFavoriteRequest>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;
        private readonly IAsyncCommandBuilder _commandBuilder;

        public SetIsFavoriteRequestHandler(
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService,
            IAsyncCommandBuilder commandBuilder 
        )
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
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
            await _commandBuilder.ExecuteAsync(new LogSetIsFavoriteCommandContext(userId, request.LogId, request.IsFavorite));
        }
    }
}
