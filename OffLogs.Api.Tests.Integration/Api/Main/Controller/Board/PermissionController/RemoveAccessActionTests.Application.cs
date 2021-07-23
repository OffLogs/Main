using System.Net;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using Xunit;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Permission;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.PermissionController
{
    public partial class RemoveAccessActionTests
    {

        [Theory]
        [InlineData(MainApiUrl.PermissionRemoveAccess)]
        public async Task ShouldRemovePermissionsOnApplication(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            await ApplicationService.ShareForUser(user.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new RemoveAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = user.ApplicationId,
                RecepientId = user2.Id
            });
            response.EnsureSuccessStatusCode();

            await DbSessionProvider.RefreshEntityAsync(user.Application);
            Assert.False(
                 await AccessPolicyService.HasReadAccessAsync(user.Application, user2)
            );
            Assert.False(
                 await AccessPolicyService.HasWriteAccessAsync(user.Application, user2)
            );
        }

        [Theory]
        [InlineData(MainApiUrl.PermissionRemoveAccess)]
        public async Task ShouldNotRemovePermissionsOnApplicationIfItAlreadyRemoved(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new RemoveAccessRequest()
            {
                AccessType = PermissionAccessType.ApplicationRead,
                ItemId = user.ApplicationId,
                RecepientId = user2.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}