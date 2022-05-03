using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification;

public record struct DeleteMessageTemplatesAction(long Id);

public record struct FetchMessageTemplatesAction(bool IsLoadIfEmpty);

public record struct FetchMessageTemplatesResultAction(ListDto<MessageTemplateDto> List);

public record struct FetchNotificationRulesAction;

public record struct FetchNotificationRulesResultAction(ListDto<NotificationRuleDto> List);

public record struct SetMessageTemplatesAction(MessageTemplateDto Item);
