using System;
using System.Collections.Generic;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class LogShareDto : IResponse
    {
        public long Id { get; set; }
        public long Token { get; set; }
        public LogShareDto()
        {
        }
    }
}
