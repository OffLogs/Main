using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Permission.Actions
{
    public class AddAccessRequestHandler : IAsyncRequestHandler<AddAccessRequest>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IApplicationService _applicationService;

        public AddAccessRequestHandler(
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IApplicationService applicationService
        )
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        }

        public async Task ExecuteAsync(AddAccessRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (request.AccessType == PermissionAccessType.ApplicationRead)
            {
                await AddApplicationReadRights(request);
            }
            var list = await _queryBuilder.For<ICollection<UserEntity>>()
                .WithAsync(new UserSearchCriteria(request.Search, new long[] { userId }));
        }

        private Task AddApplicationReadRights(AddAccessRequest request)
        {
            var recepient = await _applicationService.ShareForUser();
        }
    }
}
