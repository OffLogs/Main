using System;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Business.Db.Entity
{
    public class ApplicationEntity
    {
        public long Id { get; set; }
        public UserEntity User { get; set; }
        public string Name { get; set; }
        public string ApiToken { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        
        public ApplicationResponseModel ResponseModel
        {
            get
            {
                var model = new ApplicationResponseModel()
                {
                    Id = Id,
                    UserId = User.Id,
                    Name = Name,
                    CreateTime = CreateTime,
                };
                return model;
            }
        }
    }
}