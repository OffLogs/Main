using AutoMapper;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<ApplicationEntity, ApplicationDto>();
            CreateMap<ApplicationEntity, ApplicationListItemDto>();
        }
    }
}
