using System;
using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Entity
{
    public class LogEntity
    {
        public long Id { get; set; }
        public ApplicationEntity Application { get; set; }
        public string LevelId { get; set; }

        public LogLevel Level
        {
            get => new LogLevel().FromString(LevelId); 
            set => LevelId = value.GetValue();
        }
        public bool IsFavorite { get; set; }
        public string Message { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }
        public List<LogTraceEntity> Traces { get; set; } = new();
        public List<LogPropertyEntity> Properties { get; set; } = new();
        
        public LogResponseModel ResponseModel
        {
            get
            {
                var model = new LogResponseModel()
                {
                    Id = Id,
                    ApplicationId = Application.Id,
                    Level = Level.GetValue(),
                    Message = Message,
                    LogTime = LogTime,
                    CreateTime = CreateTime,
                };
                
                if (Traces != null)
                {
                    model.Traces = Traces.Select(item => item.Trace).ToList();
                }
                if (Properties != null)
                {
                    model.Properties = Properties.ToDictionary(
                        item => item.Key, 
                        item => item.Value
                    );
                }

                return model;
            }
        }
    }
}