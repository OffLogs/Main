using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Common.Dto;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Application;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Business.Controller.Board.Application.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, PaginatedListDto<ApplicationListItemDto>>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;

        public GetListRequestHandler(
            IJwtAuthService jwtAuthService, 
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService
        )
        {
            _jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
        }

        public async Task<PaginatedListDto<ApplicationListItemDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var applicationsList = await _queryBuilder.For<ListDto<ApplicationEntity>>()
                .WithAsync(new ApplicationGetListCriteria(userId, request.Page));

            var applicationDtos = _mapper.Map<List<ApplicationListItemDto>>(applicationsList.Items)
                .Select(dto =>
                {
                    var isHasReadAccess = _accessPolicyService.HasReadAccessAsync<ApplicationEntity>(
                    dto.Id, 
                    userId
                    ).Result;
                    var isHasWriteAccess = _accessPolicyService.HasWriteAccessAsync<ApplicationEntity>(
                        dto.Id, 
                        userId
                    ).Result;
                    dto.Permissions = new PermissionInfoDto(isHasReadAccess, isHasWriteAccess);
                    return dto;
                }).ToList();
            return new PaginatedListDto<ApplicationListItemDto>(
                applicationDtos,
                applicationsList.TotalCount
            );
        }
    }
}
