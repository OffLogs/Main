﻿using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}