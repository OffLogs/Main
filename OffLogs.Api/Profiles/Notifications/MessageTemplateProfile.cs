using System.Collections.Generic;
using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Api.Profiles.Notifications
{
    public class NotificationMessageProfile : Profile
    {
        public NotificationMessageProfile()
        {
            CreateMap<MessageTemplateEntity, MessageTemplateDto>();
            CreateMap<OffLogs.Business.Orm.Dto.ListDto<MessageTemplateEntity>, ListDto<MessageTemplateDto>>();
            CreateMap<SetMessageTemplateRequest, MessageTemplateEntity>();
        }
    }
}
