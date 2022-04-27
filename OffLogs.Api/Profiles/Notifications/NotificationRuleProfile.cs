using AutoMapper;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Api.Profiles.Notifications
{
    public class NotificationRuleProfile : Profile
    {
        public NotificationRuleProfile()
        {
            CreateMap<NotificationRuleEntity, NotificationRuleDto>();
            CreateMap<OffLogs.Business.Orm.Dto.ListDto<NotificationRuleEntity>, ListDto<NotificationRuleDto>>();
        }
    }
}
