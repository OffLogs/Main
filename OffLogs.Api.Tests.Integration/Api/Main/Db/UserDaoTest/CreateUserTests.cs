using System.Threading.Tasks;
using Npgsql;
using OffLogs.Api.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.UserDaoTest
{
    [Collection("UserDaoTest.CreateUserTests")]
    public class CreateUserTests: MyIntegrationTest
    {
        public CreateUserTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("some-user", "user@email.com")]
        [InlineData("some-user-2", "user1@email.com")]
        public async Task ShouldCreateNewUser(string expectedUserName, string expectedEmail)
        {
            await UserDao.DeleteByUserName(expectedUserName);
            
            var newUser = await UserDao.CreateNewUser(expectedUserName, expectedEmail);
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
            await UserDao.DeleteByUserName(expectedUserName);
            
            await UserDao.CreateNewUser(expectedUserName, email1);
            await Assert.ThrowsAsync<PostgresException>(() => UserDao.CreateNewUser(expectedUserName, email2));
        }
        
        [Theory]
        [InlineData("test6@test.com", "someUserName", "someUserName2")]
        [InlineData("test7@test.com", "someUserName3", "someUserName4")]
        public async Task ShouldNotCreateNewUserWithSameEmail(string expectedEmail, string userName, string userName2)
        {
            await UserDao.DeleteByUserName(userName);
            
            await UserDao.CreateNewUser(userName, expectedEmail);
            await Assert.ThrowsAsync<PostgresException>(async () =>
            {
                await UserDao.CreateNewUser(userName2, expectedEmail);
            });
        }
    }
}
