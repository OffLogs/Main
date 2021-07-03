using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Types;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Db.Entity
{
    [Class(Table = "logs")]
    public class LogEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "INT", NotNull = true)]
        public virtual long Id { get; set; }
        
        [JsonIgnore]
        private string _token { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "token", Length = 1024, NotNull = true)]
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
        [ManyToOne(
            ClassType = typeof(ApplicationEntity), 
            Column = "employee_id", 
            Lazy = Laziness.False
        )]
        public virtual ApplicationEntity Application { get; set; }
        
        [Property(TypeType = typeof(LogLevelConstantType), NotNull = true)]
        [Column(Name = "level", Length = 4, NotNull = true)]
        public virtual LogLevel Level { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "is_favorite", SqlType = "boolean", NotNull = true)]
        public virtual bool IsFavorite { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "message", Length = 2048, NotNull = true)]
        public virtual string Message { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "log_time", SqlType = "datetime")]
        public virtual DateTime LogTime { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime")]
        public virtual DateTime CreateTime { get; set; }
        
        [Bag(Inverse = true, Lazy = CollectionLazy.False, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogTraceEntity))]
        public virtual ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        
        [Bag(Inverse = true, Lazy = CollectionLazy.False, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogPropertyEntity))]
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