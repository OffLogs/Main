using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Orm.Entities;

namespace Offlogs.Business.Api.Profiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<LogEntity, LogDto>()
                .ForPath(
                    s=> s.Properties, 
                    member => member.MapFrom(
                        entity => entity.Properties.Select(
                            property => new KeyValuePair<string, string>(
                                property.Key,
                                property.Value
                            )
                        )
                    )
                )
                .ForPath(
                    s=> s.Traces, 
                    member => member.MapFrom(
                        entity => entity.Traces.Select(
                            trace => trace.Trace
                        )
                    )
                );
            CreateMap<LogEntity, LogListItemDto>();
        }
    }
}
