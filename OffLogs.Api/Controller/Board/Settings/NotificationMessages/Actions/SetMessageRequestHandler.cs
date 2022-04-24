using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using Commands.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Permission;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Settings.NotificationMessage;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.NotificationRule;
using Queries.Abstractions;
using ValidationException = OffLogs.Business.Exceptions.ValidationException;

namespace OffLogs.Api.Controller.Board.Settings.NotificationMessages.Actions
{
    public class SetMessageRequestHandler : IAsyncRequestHandler<SetMessageRequest, NotificationMessageDto>
    {
        private readonly IRequestService _requestService;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;
        private readonly IMapper _mapper;

        public SetMessageRequestHandler(
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

        public async Task<NotificationMessageDto> ExecuteAsync(SetMessageRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var message = new NotificationMessageEntity();
            if (request.Id.HasValue)
            {
                message = await _asyncQueryBuilder.FindByIdAsync<NotificationMessageEntity>(request.Id.Value);
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
            message.Subject = request.Subject;
            message.Body = request.Body;
            await _commandBuilder.SaveAsync(message);
            return _mapper.Map<NotificationMessageDto>(message);
        }
    }
}
