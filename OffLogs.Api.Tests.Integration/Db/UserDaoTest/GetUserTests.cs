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
    [Collection("UserDaoTest.GetUserTests")]
    public class GetUserTests: MyIntegrationTest
    {
        public GetUserTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("some-get-user", "user-get@email.com")]
        [InlineData("some-get-user-2", "user-get-2@email.com")]
        public async Task ShouldCreateNewUser(string expectedUserName, string expectedEmail)
        {
            await UserDao.DeleteByUserName(expectedUserName);
            
            await UserDao.CreateNewUser(expectedUserName, expectedEmail);
            var newUser = await UserDao.GetByUserName(expectedUserName);
            Assert.NotNull(newUser);
            Assert.Equal(expectedUserName, newUser.UserName);
            Assert.Equal(expectedEmail, newUser.Email);
            Assert.True(newUser.PasswordHash.Length > 0);
            Assert.True(newUser.PasswordSalt.Length > 0);
        }
    }
}
