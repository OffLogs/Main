using System.Net;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using Xunit;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Permission;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.PermissionController
{
    public partial class AddAccessActionTests
    {

        [Theory]
        [InlineData(MainApiUrl.PermissionAddAccess)]
        public async Task ShouldGrantPermissionsOnApplication(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new AddAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = user.ApplicationId,
                RecipientId = user2.Id
            });
            response.EnsureSuccessStatusCode();

            await DbSessionProvider.RefreshEntityAsync(user.Application);
            var isHasAccess = await AccessPolicyService.HasReadAccessAsync(user.Application, user2);
            Assert.True(isHasAccess);
        }

        [Theory]
        [InlineData(MainApiUrl.PermissionAddAccess)]
        public async Task ShouldNotGrantPermissionsOnApplicationIfItAlreadyGranted(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new AddAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = user.ApplicationId,
                RecipientId = user2.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}