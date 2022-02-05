using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Tests.Integration.Core.Models
{
    public class UserTestModel: UserEntity
    {
        public string PemFilePassword { get; }
        public string PemFile { get; }
        public List<ApplicationEntity> Applications { get; set; } = new();
        
        public string ApiToken { get; set; }
        
        public string ApplicationApiToken
        {
            get => Application.ApiToken;
        }
        
        public long ApplicationId
        {
            get => Application.Id;
        }
        
        public ApplicationEntity Application
        {
            get => Applications.First();
        }
        
        public UserTestModel(
            UserEntity entity,
            string pemFilePassword,
            string pemFile
        )
        {
            PemFilePassword = pemFilePassword;
            PemFile = pemFile;
            Id = entity.Id;
            UserName = entity.UserName;
            Email = entity.Email;
            PublicKey = entity.PublicKey;
            CreateTime = entity.CreateTime;
            UpdateTime = entity.UpdateTime;
        }
    }
}
