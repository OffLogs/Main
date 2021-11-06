using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Orm.Commands.Entities.User;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.User
{
    [Collection("UserDaoTest.GetUserTests")]
    public class GetUserTests : MyApiIntegrationTest
    {
        public GetUserTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData("some-get-user", "user-get@email.com")]
        [InlineData("some-get-user-2", "user-get-2@email.com")]
        public async Task ShouldCreateNewUser(string expectedUserName, string expectedEmail)
        {
            await CommandBuilder.ExecuteAsync(new UserDeleteCommandContext(expectedUserName));

            await DataSeeder.CreateActivatedUser(expectedUserName, expectedEmail);
            var newUser = await QueryBuilder.For<UserEntity>()
                .WithAsync(new UserGetByCriteria(expectedUserName));
            Assert.NotNull(newUser);
            Assert.Equal(expectedUserName, newUser.UserName);
            Assert.Equal(expectedEmail, newUser.Email);
            Assert.True(newUser.PublicKey.Length > 0);
        }
    }
}
