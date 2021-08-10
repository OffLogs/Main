using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Entities.ApplicationService
{
    public class ApplicationDeleteTests : MyApiIntegrationTest
    {
        public ApplicationDeleteTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldShareApplicationForOtherUser()
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            await ApplicationService.ShareForUser(user2.Application, user1);

            Assert.Contains(user1, user2.Application.SharedForUsers);

            // Reset current session
            await DbSessionProvider.PerformCommitAsync();
            var savedApplication = await QueryBuilder.FindByIdAsync<ApplicationEntity>(user2.Application.Id);
            var sharedUsers = savedApplication.SharedForUsers;

            Assert.Contains(user1, savedApplication.SharedForUsers);
        }

        [Fact]
        public async Task ShouldNotShareAppForOneUserTwice()
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            await ApplicationService.ShareForUser(user2.Application, user1);

            await Assert.ThrowsAsync<PermissionException>(async () => {
                await ApplicationService.ShareForUser(user2.Application, user1);
            });
        }

        [Fact]
        public async Task ShouldNotShareApplicationToTheOnwer()
        {
            var user1 = await DataSeeder.CreateNewUser();

            await Assert.ThrowsAsync<PermissionException>(async () => {
                await ApplicationService.ShareForUser(user1.Application, user1);
            });
        }
    }
}
