using System.Threading.Tasks;
using NHibernate.Exceptions;
using Npgsql;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Orm.Commands.Entities.User;
using OffLogs.Business.Orm.Exceptions;
using OffLogs.Business.Services.Entities.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.User
{
    [Collection("UserDaoTest.CreateUserTests")]
    public class CreateUserTests : MyApiIntegrationTest
    {
        public CreateUserTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData("some-user", "user@email.com")]
        [InlineData("some-user-2", "user1@email.com")]
        public async Task ShouldCreatePendingUser(string expectedUserName, string expectedEmail)
        {
            await DeleteUser(expectedUserName);

            var newUser = await UserService.CreatePendingUser(expectedEmail, expectedUserName);
            Assert.True(newUser.VerificationToken.Length > 20);
            Assert.Equal(expectedUserName, newUser.UserName);
            Assert.Equal(expectedEmail, newUser.Email);
            Assert.True(newUser.PublicKey.Length > 0);
        }

        [Theory]
        [InlineData("some-user", "test2@test.com", "test3@test.com")]
        [InlineData("some-user-2", "test4@test.com", "test5@test.com")]
        public async Task ShouldNotCreateNewUserWithSameUserName(string expectedUserName, string email1, string email2)
        {
            await DeleteUser(expectedUserName);

            await UserService.CreatePendingUser(expectedUserName, email1);
            await Assert.ThrowsAsync<EntityIsExistException>(() => UserService.CreatePendingUser(email2, expectedUserName));
        }

        [Theory]
        [InlineData("test6@test.com", "someUserName", "someUserName2")]
        [InlineData("test7@test.com", "someUserName3", "someUserName4")]
        public async Task ShouldNotCreateNewUserWithSameEmail(string expectedEmail, string userName, string userName2)
        {
            await DeleteUser(userName);

            await UserService.CreatePendingUser(expectedEmail, userName);
            await Assert.ThrowsAsync<EntityIsExistException>(async () =>
            {
                await UserService.CreatePendingUser(expectedEmail, userName2);
            });
        }

        private async Task DeleteUser(string userName)
        {
            await CommandBuilder.ExecuteAsync(new UserDeleteCommandContext(userName));
        }
    }
}
