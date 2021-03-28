using System;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Models.Response.Board
{
    public record LogPropertyResponseModel
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }

        public LogPropertyResponseModel(LogPropertyEntity entity)
        {
            Id = entity.Id;
            Key = entity.Key;
            Value = entity.Value;
            CreateTime = entity.CreateTime;
        }
    }
}