using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using Offlogs.Business.Api.Dto;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Application;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace Offlogs.Business.Api.Controller.Board.Application.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, PaginatedListDto<ApplicationListItemDto>>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;

        public GetListRequestHandler(
            IJwtAuthService jwtAuthService, 
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService
        )
        {
            _jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder;
            _requestService = requestService;
        }

        public async Task<PaginatedListDto<ApplicationListItemDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var applicationsList = await _queryBuilder.For<ListDto<ApplicationEntity>>()
                .WithAsync(new ApplicationGetListCriteria(userId, request.Page));

            var applicationDtos = _mapper.Map<List<ApplicationListItemDto>>(applicationsList.Items);
            return new PaginatedListDto<ApplicationListItemDto>(
                applicationDtos,
                applicationsList.TotalCount
            );
        }
    }
}
