using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.Log;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class GetRequestHandler : IAsyncRequestHandler<GetRequest, LogDto>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;

        public GetRequestHandler(
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

        public async Task<LogDto> ExecuteAsync(GetRequest request)
        {
            var userId = _jwtAuthService.GetUserId();
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(request.Id);
            if (log == null)
            {
                throw new ItemNotFoundException(nameof(log));
            }
            if (!await _applicationService.IsOwner(userId, log.Application))
            {
                throw new DataPermissionException();
            }

            return _mapper.Map<LogDto>(log);
        }
    }
}
