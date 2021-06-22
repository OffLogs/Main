using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Db.Entity
{
    public class LogEntity
    {
        public virtual long Id { get; set; }
        
        [JsonIgnore]
        private string _token { get; set; }

        public virtual string Token
        {
            get
            {
                if (string.IsNullOrEmpty(_token))
                {
                    _token = SecurityUtil.GetTimeBasedToken();
                }

                return _token;
            }
            set => _token = value;
        }
        
        [JsonIgnore]
        public virtual ApplicationEntity Application { get; set; }
        public virtual string LevelId { get; set; }

        [JsonIgnore]
        public virtual LogLevel Level
        {
            get => new LogLevel().FromString(LevelId); 
            set => LevelId = value.GetValue();
        }
        public virtual bool IsFavorite { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime LogTime { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        public virtual ICollection<LogPropertyEntity> Properties { get; set; } = new List<LogPropertyEntity>();
        
        [JsonIgnore]
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
                
                if (Traces != null && !Traces.IsHibernateLazy())
                {
                    model.Traces = Traces.Select(item => item.Trace).ToList();
                }
                if (Properties != null && !Properties.IsHibernateLazy())
                {
                    model.Properties = Properties.ToDictionary(
                        item => item.Key, 
                        item => item.Value
                    );
                }

                return model;
            }
        }

        public virtual void AddTrace(LogTraceEntity entity)
        {
            entity.Log = this;
            Traces.Add(entity);
        }
        
        public virtual void AddProperty(LogPropertyEntity entity)
        {
            entity.Log = this;
            Properties.Add(entity);
        }
    }
}