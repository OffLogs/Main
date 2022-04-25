using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Api.Profiles.Notifications
{
    public class NotificationConditionProfile : Profile
    {
        public NotificationConditionProfile()
        {
            CreateMap<NotificationConditionEntity, NotificationConditionDto>();
        }
    }
}
