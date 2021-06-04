using System;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Business.Db.Entity
{
    public class LogTraceEntity
    {
        public long Id { get; set; }
        public LogEntity Log { get; set; }
        public string Trace { get; set; }
        public DateTime CreateTime { get; set; }
        
        public LogTraceResponseModel ResponseModel
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