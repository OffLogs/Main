using System;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.Logging;
using Npgsql;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Helpers;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Exceptions;
using OffLogs.Business.Orm.Queries.Entities.User;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.User
{
    public class UserService: IUserService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly ILogger<UserService> _logger;
        private readonly IAsyncQueryBuilder _queryBuilder;

        public UserService(
            IAsyncCommandBuilder commandBuilder, 
            ILogger<UserService> logger,
            IAsyncQueryBuilder queryBuilder
        )
        {
            _commandBuilder = commandBuilder;
            _logger = logger;
            _queryBuilder = queryBuilder;
        }

        public async Task<UserEntity> CreateNewUser(string userName,  string email)
        {
            userName = FormatUtil.ClearUserName(userName);
            var existsUser = await _queryBuilder.For<UserEntity>()
                .WithAsync(new UserGetByCriteria(userName, email));
            if (existsUser != null)
                throw new EntityIsExistException();
            
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
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow
            };
            await _commandBuilder.SaveAsync(user);
            return user;
        }
        
        // public async Task DeleteByUserName(string userName)
        // {
        //     using var session = Session;
        //     using var transaction = session.BeginTransaction();
        //     await session.Query<UserEntity>()
        //         .Where(user => user.UserName == FormatUtil.ClearUserName(userName))
        //         .DeleteAsync();
        //     await transaction.CommitAsync();
        // }
        //
        // public async Task<UserEntity> GetByUserName(string userName)
        // {
        //     using var session = Session;
        //     var entity = await session.Query<UserEntity>()
        //         .Where(user => user.UserName == FormatUtil.ClearUserName(userName))
        //         .SingleOrDefaultAsync();
        //     return entity;
        // }
    }
}