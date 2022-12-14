using System;
using System.Text;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.Logging;
using Npgsql;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Helpers;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Exceptions;
using OffLogs.Business.Orm.Queries;
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

        public async Task<UserEntity> CreatePendingUser(string email, string userName = null)
        {
            userName ??= FormatUtil.ClearUserName(email);
            var existsUser = await _queryBuilder.For<UserEntity>()
                .WithAsync(new UserGetByCriteria(userName, email));
            if (existsUser != null)
                throw new EntityIsExistException();

            var verificationToken = SecurityUtil.GetTimeBasedToken() + SecurityUtil.GetRandomString(12);
            var user = new UserEntity
            {
                UserName = FormatUtil.ClearUserName(userName),
                Email = email.Trim().ToLower(),
                PublicKey = Array.Empty<byte>(),
                SignedData = Array.Empty<byte>(),
                Sign = Array.Empty<byte>(),
                Status = UserStatus.Pending,
                VerificationToken = verificationToken,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
            };
            await _commandBuilder.SaveAsync(user);
            return user;
        }
        
        public async Task<(UserEntity, string)> ActivateUser(
            long userId, 
            string privateKeyPassword
        )
        {
            var user = await _queryBuilder.For<UserEntity>()
                .WithAsync(new FindByIdCriteria(userId));
            if (user == null)
                throw new EntityIsNotExistException();
            if (user.IsVerified)
                throw new Exception("User already activated");
            
            // Generate private key
            var asymmetricEncryptor = AsymmetricEncryptor.GenerateKeyPair();
            
            user.Status = UserStatus.Active;
            user.VerificationTime = DateTime.UtcNow;
            user.VerificationToken = null;
            
            // Secret key initialization
            user.PublicKey = asymmetricEncryptor.GetPublicKeyBytes();
            user.SignedData = Encoding.UTF8.GetBytes(SecurityUtil.GetRandomString(48));
            user.Sign = asymmetricEncryptor.SignData(user.SignedData);
            
            await _commandBuilder.SaveAsync(user);

            var pemFileContent = asymmetricEncryptor.CreatePem(privateKeyPassword);
            return (user, pemFileContent);
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
