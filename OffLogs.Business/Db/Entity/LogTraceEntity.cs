using System;
using Newtonsoft.Json;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Business.Db.Entity
{
    public class LogTraceEntity
    {
        public virtual long Id { get; set; }
        
        [JsonIgnore]
        public virtual LogEntity Log { get; set; }
        
        public virtual string Trace { get; set; }
        
        public virtual DateTime CreateTime { get; set; }
        
        [JsonIgnore]
        public virtual LogTraceResponseModel ResponseModel
        {
            get
            {
                var model = new LogTraceResponseModel()
                {
                    Id = Id,
                    Trace = Trace,
                    CreateTime = CreateTime,
                };
                return model;
            }
        }
        
        public LogTraceEntity() {}

        public LogTraceEntity(string trace)
        {
            Trace = trace;
            CreateTime = DateTime.Now;
        }
    }
}