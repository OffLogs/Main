using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Db.Entity;
using ServiceStack.DataAnnotations;

namespace OffLogs.Api.Tests.Integration.Core.Models
{
    public class UserTestModel: UserEntity
    {
        [Ignore]
        public List<ApplicationEntity> Applications { get; set; } = new();

        [Ignore]
        public string ApiToken { get; set; }
        
        [Ignore] 
        public string ApplicationApiToken
        {
            get => Applications.First().ApiToken;
        }
        
        [Ignore] 
        public long ApplicationId
        {
            get => Application.Id;
        }
        
        [Ignore] 
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