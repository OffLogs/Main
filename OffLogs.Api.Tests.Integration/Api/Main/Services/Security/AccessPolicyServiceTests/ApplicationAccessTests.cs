using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Security.AccessPolicyServiceTests
{
    public class ApplicationAccessTests : MyApiIntegrationTest
    {
        public ApplicationAccessTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldThrowExceptionIfUserNotFound()
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await AccessPolicyService.HasWriteAccessAsync<ApplicationEntity>(user1.ApplicationId, 0);
            });
        }

        [Fact]
        public async Task ShouldThrowExceptionIfUserNotFoundByObject()
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await AccessPolicyService.HasWriteAccessAsync(user1.Application, null);
            });
        }

        [Fact]
        public async Task ShouldThrowExceptionIfApplicationNotFound()
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await AccessPolicyService.HasWriteAccessAsync<ApplicationEntity>(0, user1.Id);
            });
        }

        [Fact]
        public async Task ShouldThrowExceptionIfApplicationNotFoundByObject()
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await AccessPolicyService.HasWriteAccessAsync(null, user1);
            });
        }

        [Fact]
        public async Task OwnerShouldHasWriteAccessByIds()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var isHas = await AccessPolicyService.HasWriteAccessAsync<ApplicationEntity>(user1.ApplicationId, user1.Id);
            Assert.True(isHas);
        }

        [Fact]
        public async Task OwnerShouldHasReadAccessByIds()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var isHas = await AccessPolicyService.HasReadAccessAsync<ApplicationEntity>(user1.ApplicationId, user1.Id);
            Assert.True(isHas);
        }

        [Fact]
        public async Task OtherUserShouldNotHaveWriteAccess()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            var isHas = await AccessPolicyService.HasWriteAccessAsync(user1.Application, user2);
            Assert.False(isHas);
        }

        [Fact]
        public async Task OtherUserShouldNotHaveWriteAccessIfHasPermissions()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user1.Application, user2);

            var isHas = await AccessPolicyService.HasWriteAccessAsync(user1.Application, user2);
            Assert.False(isHas);
        }

        [Fact]
        public async Task OtherUserShouldHasReadAccess()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user1.Application, user2);

            var isHas = await AccessPolicyService.HasReadAccessAsync(user1.Application, user2);
            Assert.True(isHas);
        }
    }
}
