using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NHibernate.Criterion;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Log;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Profiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<LogEntity, LogDto>()
                .ForPath(
                    s => s.Properties,
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
                    s => s.Traces,
                    member => member.MapFrom(
                        entity => entity.Traces.Select(
                            trace => trace.Trace
                        )
                    )
                )
                .ForPath(
                    s => s.Shares,
                    member => member.MapFrom(entity => entity.LogShares)
                );

            CreateMap<LogEntity, LogSharedDto>()
                .ForPath(
                    s => s.Properties,
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
                    s => s.Traces,
                    member => member.MapFrom(
                        entity => entity.Traces.Select(
                            trace => trace.Trace
                        )
                    )
                );

            CreateMap<LogEntity, LogListItemDto>();

            // Log Statistic DTO mapping
            CreateMap<OffLogs.Business.Orm.Dto.Entities.LogStatisticForNowDto, LogStatisticForNowItemDto> ();
        }
    }
}
