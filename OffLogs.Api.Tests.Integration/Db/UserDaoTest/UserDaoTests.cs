using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OffLogs.Api.Models.Response;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Db.UserDaoTest
{
    [Collection("UserDaoTests")]
    public class UserDaoTests: MyIntegrationTest
    {
        public UserDaoTests(CustomWebApplicationFactory factory) : base(factory)
        {
            
        }
        
        [Theory]
        [InlineData("some-user", "user@email.com")]
        [InlineData("some-user-2", "user2@email.com")]
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
        [InlineData("some-user")]
        [InlineData("some-user-2")]
        public async Task ShouldNotCreateNewUserWithSameUserName(string expectedUserName)
        {
            await UserDao.DeleteByUserName(expectedUserName);
            
            await UserDao.CreateNewUser(expectedUserName);
            await Assert.ThrowsAsync<SqlException>(() => UserDao.CreateNewUser(expectedUserName));
        }
    }
}
