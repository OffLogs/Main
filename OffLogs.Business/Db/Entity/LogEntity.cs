using System;
using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;
using ServiceStack.DataAnnotations;

namespace OffLogs.Business.Db.Entity
{
    [Alias("logs")]
    public class LogEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }
        
        [Alias("application_id")]
        [References(typeof(ApplicationEntity))]
        public long ApplicationId { get; set; }
        
        [Alias("level")]
        public LogLevel Level { get; set; }
        
        [Alias("is_favorite")]
        public bool IsFavorite { get; set; }
        
        [Alias("message")]
        public string Message { get; set; }
        
        [Alias("log_time")]
        public DateTime LogTime { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }
        
        [Reference]
        public ApplicationEntity Application { get; set; }
        
        [Reference]
        public List<LogTraceEntity> Traces { get; set; } = new();

        [Reference] 
        public List<LogPropertyEntity> Properties { get; set; } = new();

        [Computed]
        public LogResponseModel ResponseModel
        {
            get
            {
                var model = new LogResponseModel()
                {
                    Id = Id,
                    ApplicationId = ApplicationId,
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