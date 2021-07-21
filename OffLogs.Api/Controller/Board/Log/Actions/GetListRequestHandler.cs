using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, PaginatedListDto<LogListItemDto>>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;

        public GetListRequestHandler(
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
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        public async Task<PaginatedListDto<LogListItemDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _applicationService.IsOwner(userId, request.ApplicationId))
            {
                throw new DataPermissionException();
            }

            var list = await _queryBuilder.For<ListDto<LogEntity>>()
                .WithAsync(new LogGetListCriteria { 
                    ApplicationId = request.ApplicationId,
                    LogLevel = request.LogLevel,
                    Page = request.Page
                });

            var applicationDtos = _mapper.Map<List<LogListItemDto>>(list.Items);
            return new PaginatedListDto<LogListItemDto>(
                applicationDtos,
                list.TotalCount
            );
        }
    }
}
