using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.NotificationRule;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Notifications.Rules.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, ListDto<NotificationRuleDto>>
    {
        private readonly IRequestService _requestService;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IMapper _mapper;

        public GetListRequestHandler(
            IRequestService requestService,
            IAsyncQueryBuilder queryBuilder,
            IMapper mapper
        )
        {
            _requestService = requestService;
            _queryBuilder = queryBuilder;
            _mapper = mapper;
        }

        public async Task<ListDto<NotificationRuleDto>> ExecuteAsync(GetListRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            
            var rules = await _queryBuilder.For<Business.Orm.Dto.ListDto<NotificationRuleEntity>>()
                .WithAsync(
                    new FindByIdCriteria(userId)    
                );
            return _mapper.Map<ListDto<NotificationRuleDto>>(rules);
        }
    }
}
