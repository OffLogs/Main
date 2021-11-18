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
            CreateMap<ApplicationEntity, ApplicationDto>()
                .ForPath(
                    s => s.PublicKeyBase64,
                    member => member.MapFrom(
                        entity => Convert.ToBase64String(entity.PublicKey)
                    )
                )
                .ForPath(
                    s => s.EncryptedPrivateKeyBase64,
                    member => member.MapFrom(
                        entity => Convert.ToBase64String(entity.EncryptedPrivateKey)
                    )
                );
            CreateMap<ApplicationEntity, ApplicationListItemDto>();
        }
    }
}
