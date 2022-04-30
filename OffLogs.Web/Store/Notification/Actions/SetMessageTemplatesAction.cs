using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification.Actions;

public class SetMessageTemplatesAction
{
    public MessageTemplateDto Item { get; }

    public SetMessageTemplatesAction(MessageTemplateDto item)
    {
        Item = item;
    }
}
