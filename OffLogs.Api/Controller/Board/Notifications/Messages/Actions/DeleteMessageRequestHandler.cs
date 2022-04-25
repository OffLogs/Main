using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using Commands.Abstractions;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using Queries.Abstractions;
using ValidationException = OffLogs.Business.Exceptions.ValidationException;

namespace OffLogs.Api.Controller.Board.Notifications.Messages.Actions
{
    public class DeleteMessageRequestHandler : IAsyncRequestHandler<IdRequest>
    {
        private readonly IRequestService _requestService;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteMessageRequestHandler> _logger;

        public DeleteMessageRequestHandler(
            IRequestService requestService,
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder asyncQueryBuilder,
            IMapper mapper,
            ILogger<DeleteMessageRequestHandler> logger
            )
        {
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            _asyncQueryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync(IdRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var message = await _asyncQueryBuilder.FindByIdAsync<NotificationMessageEntity>(request.Id);
            if (message == null)
            {
                throw new ItemNotFoundException("Message not found");
            }
            if (!message.IsOwner(userId))
            {
                throw new PermissionException("User has no permissions to delete this message");
            }

            try
            {
                await _commandBuilder.DeleteAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw new ValidationException("The record can not be deleted");
            }
        }
    }
}
