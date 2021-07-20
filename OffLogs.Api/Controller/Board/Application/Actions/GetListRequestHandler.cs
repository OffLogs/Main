using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Application;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Application.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, PaginatedListDto<ApplicationListItemDto>>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;

        public GetListRequestHandler(
            IJwtAuthService jwtAuthService, 
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder
        )
        {
            this._jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._queryBuilder = queryBuilder;
        }

        public async Task<PaginatedListDto<ApplicationListItemDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _jwtAuthService.GetUserId();
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
