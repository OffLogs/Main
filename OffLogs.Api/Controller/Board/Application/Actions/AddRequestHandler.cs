using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Application.Actions
{
    public class AddRequestHandler : IAsyncRequestHandler<AddRequest, ApplicationDto>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;

        public AddRequestHandler(
            IJwtAuthService jwtAuthService,
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService
        )
        {
            this._jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            this._applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        }

        public async Task<ApplicationDto> ExecuteAsync(AddRequest request)
        {
            var userId = _jwtAuthService.GetUserId();
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            var application = await _applicationService.CreateNewApplication(user, request.Name);
            return _mapper.Map<ApplicationDto>(application);
        }
    }
}
