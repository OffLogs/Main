using System.Threading.Tasks;
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
        [InlineData("user-get@email.com")]
        [InlineData("user-get-2@email.com")]
        public async Task ShouldCreateNewUser(string expectedEmail)
        {
            await CommandBuilder.ExecuteAsync(new UserDeleteCommandContext(null, expectedEmail));

            await DataSeeder.CreateActivatedUser(expectedEmail);
            var newUser = await QueryBuilder.For<UserEntity>()
                .WithAsync(new UserGetByCriteria(null, expectedEmail));
            Assert.NotNull(newUser);
            Assert.Equal(expectedEmail, newUser.Email);
            Assert.True(newUser.PublicKey.Length > 0);
        }
    }
}
