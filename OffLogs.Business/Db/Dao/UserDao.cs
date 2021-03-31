using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Helpers;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

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
                UserName = FormatUtil.ClearUserName(userName),
                Email = email,
                Password = password,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            user.Id = await Connection.InsertAsync(user, selectIdentity: true);
            return user;
        }
        
        public async Task DeleteByUserName(string userName)
        {
            await Connection.DeleteAsync<UserEntity>(user => user.UserName == FormatUtil.ClearUserName(userName));
        }
        
        public async Task<UserEntity> GetByUserName(string userName)
        {
            return await Connection.SingleAsync<UserEntity>(user => user.UserName == FormatUtil.ClearUserName(userName));
        }
    }
}