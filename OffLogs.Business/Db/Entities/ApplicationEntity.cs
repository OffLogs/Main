using System;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Business.Db.Entity
{
    [Class(Table = "applications")]
    public class ApplicationEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "INT", NotNull = true)]
        public virtual long Id { get; set; }
        
        [ManyToOne(
            ClassType = typeof(UserEntity), 
            Column = "user_id", 
            Lazy = Laziness.False,
            Cascade = "delete-orphan"
        )]
        public virtual UserEntity User { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "name", Length = 200, NotNull = true)]
        public virtual string Name { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "api_token", Length = 2048, NotNull = true)]
        public virtual string ApiToken { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
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