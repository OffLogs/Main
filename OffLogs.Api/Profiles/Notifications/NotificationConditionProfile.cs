using System;
using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Api.Profiles.Notifications
{
    public class NotificationConditionProfile : Profile
    {
        public NotificationConditionProfile()
        {
            CreateMap<NotificationConditionEntity, NotificationConditionDto>();
            CreateMap<SetConditionRequest, NotificationConditionEntity>()
                .ForPath(
                    s => s.ConditionField,
                    member => member.MapFrom(
                        entity => Enum.Parse<ConditionFieldType>(entity.ConditionField)
                    )
                );
        }
    }
}
