using System.Threading.Tasks;
using Npgsql;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Orm.Commands.Entities.User;
using OffLogs.Business.Services.Entities.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.UserDaoTest
{
    [Collection("UserDaoTest.CreateUserTests")]
    public class CreateUserTests: MyApiIntegrationTest
    {
        public CreateUserTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("some-user", "user@email.com")]
        [InlineData("some-user-2", "user1@email.com")]
        public async Task ShouldCreateNewUser(string expectedUserName, string expectedEmail)
        {
            await DeleteUser(expectedUserName);
            
            var newUser = await UserService.CreateNewUser(expectedUserName, expectedEmail);
            Assert.True(newUser.Password.Length > 3);
            Assert.Equal(expectedUserName, newUser.UserName);
            Assert.Equal(expectedEmail, newUser.Email);
            Assert.True(newUser.PasswordHash.Length > 0);
            Assert.True(newUser.PasswordSalt.Length > 0);
        }
        
        [Theory]
        [InlineData("some-user", "test2@test.com", "test3@test.com")]
        [InlineData("some-user-2", "test4@test.com", "test5@test.com")]
        public async Task ShouldNotCreateNewUserWithSameUserName(string expectedUserName, string email1, string email2)
        {
            await DeleteUser(expectedUserName);
            
            await UserService.CreateNewUser(expectedUserName, email1);
            await Assert.ThrowsAsync<PostgresException>(() => UserService.CreateNewUser(expectedUserName, email2));
        }
        
        [Theory]
        [InlineData("test6@test.com", "someUserName", "someUserName2")]
        [InlineData("test7@test.com", "someUserName3", "someUserName4")]
        public async Task ShouldNotCreateNewUserWithSameEmail(string expectedEmail, string userName, string userName2)
        {
            await DeleteUser(userName);
            
            await UserService.CreateNewUser(userName, expectedEmail);
            await Assert.ThrowsAsync<PostgresException>(async () =>
            {
                await UserService.CreateNewUser(userName2, expectedEmail);
            });
        }

        private async Task DeleteUser(string userName)
        {
            await CommandBuilder.ExecuteAsync(new UserDeleteCommandContext(userName));
        }
    }
}
