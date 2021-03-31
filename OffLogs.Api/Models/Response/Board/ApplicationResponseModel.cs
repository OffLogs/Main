using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Models.Response.Board
{
    public record ApplicationResponseModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public ApplicationResponseModel()
        {
        }

        public ApplicationResponseModel(ApplicationEntity entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            Name = entity.Name;
            CreateTime = entity.CreateTime;
        }
    }
}