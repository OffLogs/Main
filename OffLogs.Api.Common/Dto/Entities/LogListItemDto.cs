using System;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class LogListItemDto : IResponse
    {
        public long Id { get; set; }
        public long ApplicationId { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }

        public LogListItemDto()
        {
        }
    }
}
