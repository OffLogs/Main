using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Notifications.Messages.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetMessageTemplateListRequest, ListDto<MessageTemplateDto>>
    {
        private readonly IRequestService _requestService;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IMapper _mapper;

        public GetListRequestHandler(
            IRequestService requestService,
            IAsyncQueryBuilder asyncQueryBuilder,
            IMapper mapper
        )
        {
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _queryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
            _mapper = mapper;
        }

        public async Task<ListDto<MessageTemplateDto>> ExecuteAsync(GetMessageTemplateListRequest templateRequest)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var templates = await _queryBuilder.For<Business.Orm.Dto.ListDto<MessageTemplateEntity>>()
                .WithAsync(
                    new FindByIdCriteria(userId)    
                );
            return _mapper.Map<ListDto<MessageTemplateDto>>(templates);
        }
    }
}
