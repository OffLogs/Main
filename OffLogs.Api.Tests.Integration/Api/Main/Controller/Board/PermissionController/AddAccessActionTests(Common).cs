using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Controller.Board.Permission.Actions;
using OffLogs.Business.Common.Constants;
using Xunit;
using OffLogs.Business.Common.Constants.Permissions;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.PermissionController
{
    public partial class AddAccessActionTests : MyApiIntegrationTest
    {
        public AddAccessActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData(MainApiUrl.PermissionAddAccess)]
        public async Task OnlyAuthorizedUsersCanDoIt(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new AddAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = user.ApplicationId,
                RecepientId = user2.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.PermissionAddAccess)]
        public async Task ShouldReturnErrorIfAccessTypeIsIncorrect(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new AddAccessRequest()
            {
                AccessType = 0,
                ItemId = user.ApplicationId,
                RecepientId = user2.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(MainApiUrl.PermissionAddAccess)]
        public async Task ShouldReturnErrorIfItemIdIsIncorrect(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new AddAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = 0,
                RecepientId = user2.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(MainApiUrl.PermissionAddAccess)]
        public async Task ShouldReturnErrorIfRecepientIdIsIncorrect(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new AddAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = 0,
                RecepientId = user2.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}