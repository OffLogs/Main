using AutoMapper;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Profiles
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
