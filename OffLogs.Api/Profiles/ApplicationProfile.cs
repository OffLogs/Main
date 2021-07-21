using AutoMapper;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Profiles
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
