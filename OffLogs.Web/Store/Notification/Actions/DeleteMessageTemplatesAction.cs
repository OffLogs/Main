using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification.Actions;

public class DeleteMessageTemplatesAction
{
    public long Id { get; }

    public DeleteMessageTemplatesAction(long id)
    {
        Id = id;
    }
}
