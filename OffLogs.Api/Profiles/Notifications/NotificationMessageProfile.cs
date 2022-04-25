using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Settings.NotificationMessage;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Api.Profiles.Notifications
{
    public class NotificationMessageProfile : Profile
    {
        public NotificationMessageProfile()
        {
            CreateMap<NotificationMessageEntity, NotificationMessageDto>();
            CreateMap<SetMessageRequest, NotificationMessageEntity>();
        }
    }
}
