using System;
using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Entity
{
    public class LogEntity
    {
        public virtual long Id { get; set; }
        public virtual ApplicationEntity Application { get; set; }
        public virtual string LevelId { get; set; }

        public virtual LogLevel Level
        {
            get => new LogLevel().FromString(LevelId); 
            set => LevelId = value.GetValue();
        }
        public virtual bool IsFavorite { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime LogTime { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual List<LogTraceEntity> Traces { get; set; } = new();
        public virtual List<LogPropertyEntity> Properties { get; set; } = new();
        
        public virtual LogResponseModel ResponseModel
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