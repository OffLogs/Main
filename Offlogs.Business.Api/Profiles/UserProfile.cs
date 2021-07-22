using AutoMapper;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace Offlogs.Business.Api.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
