using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Tests.Integration.Core.Models
{
    public class UserTestModel: UserEntity
    {
        [Computed] 
        public List<ApplicationEntity> Applications { get; set; } = new();

        [Computed] 
        public string ApiToken { get; set; }
        
        public UserTestModel(UserEntity entity)
        {
            Id = entity.Id;
            UserName = entity.UserName;
            Email = entity.Email;
            Password = entity.Password;
            PasswordHash = entity.PasswordHash;
            PasswordSalt = entity.PasswordSalt;
            CreateTime = entity.CreateTime;
            UpdateTime = entity.UpdateTime;
        }
    }
}