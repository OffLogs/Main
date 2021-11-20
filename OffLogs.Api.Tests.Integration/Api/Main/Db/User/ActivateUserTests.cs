using System;
using System.Threading.Tasks;
using NHibernate.Exceptions;
using Npgsql;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Commands.Entities.User;
using OffLogs.Business.Orm.Exceptions;
using OffLogs.Business.Services.Entities.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.User
{
    [Collection("UserDaoTest.CreateUserTests")]
    public class ActivateUserTests : MyApiIntegrationTest
    {
        public ActivateUserTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldActivatePendingUser()
        {
            var newUser = DataFactory.UserFactory().Generate();
            var expectedUser = await UserService.CreatePendingUser(newUser.Email);
            Assert.False(expectedUser.IsVerificated);
            
            var (actualUser, pemFile) = await UserService.ActivateUser(
                expectedUser.Id,
                SecurityUtil.GeneratePassword()
            );
            Assert.True(actualUser.IsVerificated);
            
            Assert.Equal(actualUser.Id, expectedUser.Id);
            Assert.True(newUser.PublicKey.Length > 0);
            Assert.NotEmpty(pemFile);
        }

        [Fact]
        public async Task ShouldNotActivateUserIfNotExists()
        {
            await Assert.ThrowsAsync<EntityIsNotExistException>(async () =>
            {
                await UserService.ActivateUser(
                    99999,
                    SecurityUtil.GeneratePassword()
                );
            });
        }

        [Fact]
        public async Task ShouldNotActivateUserIfItAlreadyActivated()
        {
            var newUser = DataFactory.UserFactory().Generate();
            var expectedUser = await UserService.CreatePendingUser(newUser.Email);
            Assert.False(expectedUser.IsVerificated);
            
            await UserService.ActivateUser(
                expectedUser.Id,
                SecurityUtil.GeneratePassword()
            );
            
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await UserService.ActivateUser(
                    expectedUser.Id,
                    SecurityUtil.GeneratePassword()
                );
            });
        }
    }
}
