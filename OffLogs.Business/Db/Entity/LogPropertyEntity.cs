using System;
using Bogus.DataSets;
using Newtonsoft.Json;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Business.Db.Entity
{
    public class LogPropertyEntity
    {
        public long Id { get; set; }
        public LogEntity Log { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }
        
        public LogPropertyResponseModel ResponseModel
        {
            get
            {
                var model = new LogPropertyResponseModel()
                {
                    Id = Id,
                    Key = Key,
                    Value = Value,
                    CreateTime = CreateTime,
                };
                return model;
            }
        }
        
        public LogPropertyEntity() {}

        public LogPropertyEntity(string key, string value)
        {
            Key = key;
            Value = value;
            CreateTime = DateTime.Now;
        }
        
        public LogPropertyEntity(string key, object value)
        {
            Key = key;
            try
            {
                Value = JsonConvert.SerializeObject(value);
            }
            catch (Exception e)
            {
                Value = "";
            }
            CreateTime = DateTime.Now;
        }
    }
}