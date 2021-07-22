using AutoMapper;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace Offlogs.Business.Api.Profiles
{
    public class ApplicationProfile: Profile
    {
        public ApplicationProfile()
        {
            CreateMap<ApplicationEntity, ApplicationDto>();
            CreateMap<ApplicationEntity, ApplicationListItemDto>();
        }
    }
}
