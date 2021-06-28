using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using Npgsql;
using OffLogs.Business.Constants;
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
                UserName = FormatUtil.ClearUserName(userName),
                Email = email,
                Password = password,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            using var session = Session;
            using var transaction = session.BeginTransaction();
            try
            {
                user.Id = (long)await session.SaveAsync(user);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Logger.LogError(e.Message, e);
                throw new PostgresException(e.Message, e.StackTrace, "", "");
            }
            return user;
        }
        
        public async Task DeleteByUserName(string userName)
        {
            using var session = Session;
            using var transaction = session.BeginTransaction();
            await session.Query<UserEntity>()
                .Where(user => user.UserName == FormatUtil.ClearUserName(userName))
                .DeleteAsync();
            await transaction.CommitAsync();
        }
        
        public async Task<UserEntity> GetByUserName(string userName)
        {
            using var session = Session;
            var entity = await session.Query<UserEntity>()
                .Where(user => user.UserName == FormatUtil.ClearUserName(userName))
                .SingleOrDefaultAsync();
            return entity;
        }
    }
}