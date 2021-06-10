using System;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Business.Db.Entity
{
    public class ApplicationEntity
    {
        public virtual long Id { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual string Name { get; set; }
        public virtual string ApiToken { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime UpdateTime { get; set; }
        
        public virtual ApplicationResponseModel ResponseModel
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
        
        public ApplicationEntity() {}
    }
}