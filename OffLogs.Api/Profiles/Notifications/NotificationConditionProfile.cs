using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Api.Profiles.Notifications
{
    public class NotificationConditionProfile : Profile
    {
        public NotificationConditionProfile()
        {
            CreateMap<NotificationConditionEntity, NotificationConditionDto>();
            CreateMap<SetConditionRequest, NotificationConditionEntity>();
        }
    }
}
