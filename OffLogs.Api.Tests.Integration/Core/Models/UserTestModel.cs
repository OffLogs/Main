using System;
using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Api.Tests.Integration.Core.Models
{
    public class UserTestModel: UserEntity
    {
        public string PemFilePassword { get; }
        
        public string PemFile { get; }

        private string _privateKeyBase64;
        public string PrivateKeyBase64
        {
            get
            {
                if (string.IsNullOrEmpty(_privateKeyBase64))
                {
                    var encryptor = AsymmetricEncryptor.ReadFromPem(PemFile, PemFilePassword);
                    _privateKeyBase64 = Convert.ToBase64String(encryptor.GetPrivateKeyBytes());
                }

                return _privateKeyBase64;
            }
        }
        
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
