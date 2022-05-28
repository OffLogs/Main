using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification;

#region Templates

public record struct DeleteMessageTemplatesAction(long Id);

public record struct FetchMessageTemplatesAction(bool IsLoadIfEmpty);

public record struct FetchMessageTemplatesResultAction(ListDto<MessageTemplateDto> List);

public record struct SetMessageTemplatesAction(MessageTemplateDto Item);

#endregion

#region Rules

public record struct FetchNotificationRulesAction;

public record struct FetchNotificationRulesResultAction(ListDto<NotificationRuleDto> List);

public record struct SetNotificationRuleAction(NotificationRuleDto Item);

public record struct DeleteNotificationRuleAction(long Id);

public record struct SetIsLoading(bool IsLoading);

#endregion
