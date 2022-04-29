using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Notification.Actions;

public class FetchMessageTemplatesResultAction
{
    public ListDto<MessageTemplateDto> List { get; }

    public FetchMessageTemplatesResultAction(ListDto<MessageTemplateDto> list)
    {
        List = list;
    }
}
