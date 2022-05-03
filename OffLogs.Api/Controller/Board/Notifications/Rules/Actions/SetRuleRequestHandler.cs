using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
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
    public class SetRuleRequestHandler : IAsyncRequestHandler<SetRuleRequest, NotificationRuleDto>
    {
        private readonly IRequestService _requestService;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IMapper _mapper;
        private readonly INotificationRuleService _notificationRuleService;

        public SetRuleRequestHandler(
            IRequestService requestService,
            IAsyncQueryBuilder queryBuilder,
            IMapper mapper,
            INotificationRuleService notificationRuleService
        )
        {
            _requestService = requestService;
            _queryBuilder = queryBuilder;
            _mapper = mapper;
            _notificationRuleService = notificationRuleService;
        }

        public async Task<NotificationRuleDto> ExecuteAsync(SetRuleRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            var message = await _queryBuilder.FindByIdAsync<MessageTemplateEntity>(request.MessageId);
            
            if (message == null) throw new ItemNotFoundException("MessageId is incorrect");
            
            ApplicationEntity application = null;
            if (request.ApplicationId.HasValue)
            {
                application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(request.ApplicationId.Value);    
            }
            if (application != null && !application.IsOwner(user.Id))
                throw new PermissionException("User has no permissions");

            Enum.TryParse<LogicOperatorType>(request.LogicOperator, out var logicOperator);
            Enum.TryParse<NotificationType>(request.Type, out var notificationType);
            var rule = await _notificationRuleService.SetRule(
                user,
                request.Period,
                logicOperator,
                notificationType,
                message,
                _mapper.Map<ICollection<NotificationConditionEntity>>(request.Conditions),
                application,
                request.Id
            );
            return _mapper.Map<NotificationRuleDto>(rule);
        }
    }
}
