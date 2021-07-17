using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Db.Entities
{
    [Class(Table = "logs", NameType = typeof(LogEntity))]
    public class LogEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
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
            Column = "application_id", 
            Lazy = Laziness.False,
            Fetch = FetchMode.Join
        )]
        public virtual ApplicationEntity Application { get; set; }

        [Property(TypeType = typeof(LogLevel), NotNull = true)]
        [Column(Name = "level", SqlType = "int", NotNull = true)]
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
        [Column(Name = "create_time", SqlType = "datetime", NotNull = false)]
        public virtual DateTime CreateTime { get; set; }
        
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogTraceEntity))]
        public virtual ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogPropertyEntity))]
        public virtual ICollection<LogPropertyEntity> Properties { get; set; } = new List<LogPropertyEntity>();

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

        public virtual LogResponseModel GetResponseModel()
        {
            var model = new LogResponseModel()
            {
                Id = Id,
                ApplicationId = Application.Id,
                Level = Level,
                Message = Message,
                IsFavorite = IsFavorite,
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
}