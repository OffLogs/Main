using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Security;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, PaginatedListDto<LogListItemDto>>
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IRequestService _requestService;
        private readonly IAccessPolicyService _accessPolicyService;
        private readonly ILogService _logService;
        private readonly ILogAssembler _logAssembler;

        public GetListRequestHandler(
            IJwtAuthService jwtAuthService,
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IRequestService requestService,
            IAccessPolicyService accessPolicyService,
            ILogService logService,
            ILogAssembler logAssembler
        )
        {
            _jwtAuthService = jwtAuthService ?? throw new ArgumentNullException(nameof(jwtAuthService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _accessPolicyService = accessPolicyService ?? throw new ArgumentNullException(nameof(accessPolicyService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _logAssembler = logAssembler;
        }

        public async Task<PaginatedListDto<LogListItemDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            if (
                !await _accessPolicyService.HasReadAccessAsync<ApplicationEntity>(
                    request.ApplicationId, 
                    userId
                )
            )
            {
                throw new DataPermissionException();
            }

            var privateKey = Convert.FromBase64String(request.PrivateKeyBase64);
            var encryptedList = await _queryBuilder.For<Business.Orm.Dto.ListDto<LogEntity>>()
                .WithAsync(new LogGetListCriteria { 
                    ApplicationId = request.ApplicationId,
                    LogLevel = request.LogLevel,
                    Page = request.Page,
                    FavoriteForUserId = request.IsFavorite ? userId : null,
                    CreateTimeFrom = request.CreateTimeFrom,
                    CreateTimeTo = request.CreateTimeTo
                });
            var list = new List<LogEntity>();
            foreach (var log in encryptedList.Items)
            {
                list.Add(await _logAssembler.AssembleDecryptedLogAsync(log, privateKey));
            }

            var responseItems = _mapper.Map<List<LogListItemDto>>(list);
            return new PaginatedListDto<LogListItemDto>(
                responseItems,
                encryptedList.TotalCount
            );
        }
    }
}
