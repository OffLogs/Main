using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Services.Api;
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
    public class UpdateRequestHandler : IAsyncRequestHandler<UpdateRequest, ApplicationDto>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;

        public UpdateRequestHandler(
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IRequestService requestService
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        public async Task<ApplicationDto> ExecuteAsync(UpdateRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (!await _applicationService.IsOwner(userId, request.Id))
            {
                throw new DataPermissionException();
            }
            var application = await _applicationService.UpdateApplication(request.Id, request.Name);
            return _mapper.Map<ApplicationDto>(application);
        }
    }
}
