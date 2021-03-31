using System;
using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Models.Response.Board
{
    public record LogResponseModel
    {
        public long Id { get; }
        public long ApplicationId { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }
        
        public List<string> Traces { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();

        public LogResponseModel()
        {
        }

        public LogResponseModel(LogEntity entity)
        {
            Id = entity.Id;
            ApplicationId = entity.ApplicationId;
            Level = entity.Level.GetValue();
            Message = entity.Message;
            LogTime = entity.LogTime;
            CreateTime = entity.CreateTime;
            if (entity.Traces != null)
            {
                Traces = entity.Traces.Select(item => item.Trace).ToList();
            }
            if (entity.Properties != null)
            {
                Properties = entity.Properties.ToDictionary(
                    item => item.Key, 
                    item => item.Value
                );
            }
        }
    }
}