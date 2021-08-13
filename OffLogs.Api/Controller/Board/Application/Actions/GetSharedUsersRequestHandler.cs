using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Application;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Business.Controller.Board.Application.Actions
{
    public class GetSharedUsersRequestHandler : IAsyncRequestHandler<GetSharedUsersRequest, UsersListDto>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;

        public GetSharedUsersRequestHandler(
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
        }

        public async Task<UsersListDto> ExecuteAsync(GetSharedUsersRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(request.Id);
            if (application == null)
            {
                throw new ItemNotFoundException(request.Id);
            }
            var isWriteAccess = await _accessPolicyService.HasWriteAccessAsync<ApplicationEntity>(application.Id, userId);
            if (!isWriteAccess)
            {
                throw new DataPermissionException();
            }
            
            
            return _mapper.Map<UsersListDto>(application.SharedForUsers);
        }
    }
}
