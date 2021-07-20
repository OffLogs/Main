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
    public class SetIsFavoriteRequestHandler : IAsyncRequestHandler<SetIsFavoriteRequest>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly IApplicationService _applicationService;
        private readonly IAsyncQueryBuilder _queryBuilder;

        public SetIsFavoriteRequestHandler(
            IJwtAuthService jwtAuthService,
            IMapper mapper,
            ILogService logService,
            IApplicationService applicationService,
            IAsyncQueryBuilder queryBuilder
        )
        {
            this._jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logService = logService;
            _applicationService = applicationService;
            _queryBuilder = queryBuilder;
        }

        public async Task ExecuteAsync(SetIsFavoriteRequest request)
        {
            var userId = _jwtAuthService.GetUserId();
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(request.LogId);
            if (log == null)
            {
                throw new ItemNotFoundException(nameof(log));
            }
            if (!await _applicationService.IsOwner(userId, log.Application))
            {
                throw new DataPermissionException();
            }
            await _logService.SetIsFavoriteAsync(request.LogId, request.IsFavorite);
        }
    }
}
