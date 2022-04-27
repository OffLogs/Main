using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using Commands.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.Notifications.Messages.Actions
{
    public class SetMessageTemplateRequestHandler : IAsyncRequestHandler<SetMessageTemplateRequest, MessageTemplateDto>
    {
        private readonly IRequestService _requestService;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;
        private readonly IMapper _mapper;

        public SetMessageTemplateRequestHandler(
            IRequestService requestService,
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder asyncQueryBuilder,
            IMapper mapper
        )
        {
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            _asyncQueryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
            _mapper = mapper;
        }

        public async Task<MessageTemplateDto> ExecuteAsync(SetMessageTemplateRequest templateRequest)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var message = new MessageTemplateEntity();
            if (templateRequest.Id.HasValue)
            {
                message = await _asyncQueryBuilder.FindByIdAsync<MessageTemplateEntity>(templateRequest.Id.Value);
                if (!message.IsOwner(userId))
                {
                    throw new PermissionException("User has no permissions to change this message");
                }
            }
            else
            {
                message.User = await _asyncQueryBuilder.FindByIdAsync<UserEntity>(userId);
                message.CreateTime = DateTime.UtcNow;
            }

            message.UpdateTime = DateTime.UtcNow;
            message.Subject = templateRequest.Subject;
            message.Body = templateRequest.Body;
            await _commandBuilder.SaveAsync(message);
            return _mapper.Map<MessageTemplateDto>(message);
        }
    }
}
