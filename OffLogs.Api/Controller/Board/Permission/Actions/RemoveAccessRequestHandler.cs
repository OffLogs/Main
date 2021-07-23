using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Permission;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using Queries.Abstractions;
using ValidationException = OffLogs.Business.Exceptions.ValidationException;

namespace OffLogs.Api.Business.Controller.Board.Permission.Actions
{
    public class RemoveAccessRequestHandler : IAsyncRequestHandler<RemoveAccessRequest>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IApplicationService _applicationService;

        public RemoveAccessRequestHandler(
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IApplicationService applicationService
        )
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        }

        public async Task ExecuteAsync(RemoveAccessRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (request.AccessType == PermissionAccessType.ApplicationRead)
            {
                await RemoveApplicationReadRights(request);
            }
            else
            {
                throw new ValidationException("Handler for the current AccessType not found");
            }
        }

        private async Task RemoveApplicationReadRights(RemoveAccessRequest request)
        {
            var recepient = await _queryBuilder.FindByIdAsync<UserEntity>(request.RecepientId);
            if (recepient == null)
                throw new ItemNotFoundException(nameof(recepient));
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(request.ItemId);
            if (application == null)
                throw new ItemNotFoundException(nameof(application));

            await _applicationService.RemoveShareForUser(application, recepient);
        }
    }
}
