using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Controller.Board.Application.Dto;
using OffLogs.Api.Dto;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using OffLogs.Business.Services.Jwt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Application.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, PaginatedListDto<ApplicationDto>>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;

        public GetListRequestHandler(IJwtAuthService jwtAuthService, IMapper mapper)
        {
            this._jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<PaginatedListDto<ApplicationDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _jwtAuthService.GetUserId();

            return Task.CompletedTask;
        }
    }
}
