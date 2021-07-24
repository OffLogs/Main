using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Profiles
{
    public class LogShareProfile : Profile
    {
        public LogShareProfile()
        {

            CreateMap<LogShareEntity, LogShareDto>();
        }
    }
}
