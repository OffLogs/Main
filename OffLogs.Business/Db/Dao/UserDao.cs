using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Db.Dao
{
    public class UserDao: CommonDao, IUserDao
    {
        public UserDao(IConfiguration configuration, ILogger<UserDao> logger) : base(
            configuration,
            logger
        )
        {
        }

        public async Task<UserEntity> CreateNewUser(string userName,  string email)
        {
            var password = SecurityUtil.GeneratePassword(8);
            var passwordSalt = SecurityUtil.GenerateSalt();
            var passwordHash = SecurityUtil.GeneratePasswordHash(password, passwordSalt);
            var user = new UserEntity()
            {
                UserName = userName,
                Email = email,
                Password = password,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            await Connection.InsertAsync(user);
            return user;
        }
    }
}