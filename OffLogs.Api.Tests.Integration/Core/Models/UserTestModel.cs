using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Tests.Integration.Core.Models
{
    public class UserTestModel: UserEntity
    {
        public List<ApplicationEntity> Applications { get; set; } = new();
        
        public string ApiToken { get; set; }
        
        public string ApplicationApiToken
        {
            get => Applications.First().ApiToken;
        }
        
        public long ApplicationId
        {
            get => Application.Id;
        }
        
        public ApplicationEntity Application
        {
            get => Applications.First();
        }
        
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