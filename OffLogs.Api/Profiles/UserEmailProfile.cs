using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Profiles
{
    public class UserEmailProfile : Profile
    {
        public UserEmailProfile()
        {
            CreateMap<UserEmailEntity, UserEmailDto>();
            CreateMap<UserEmailEntity, UserEmailDto>();
        }
    }
}
