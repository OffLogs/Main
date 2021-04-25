using System;

namespace OffLogs.Business.Common.Models.Api.Response.Board
{
    public record LogPropertyResponseModel
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }
    }
}