using System;
using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<ApplicationEntity, ApplicationDto>();
            CreateMap<ApplicationEntity, ApplicationListItemDto>();
            CreateMap<OffLogs.Business.Orm.Dto.Entities.ApplicationStatisticDto, ApplicationStatisticDto>();
        }
    }
}
