using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class GetSharedUsersActionTests : MyApiIntegrationTest
    {
        public GetSharedUsersActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetSharedUser)]
        public async Task OnlyAuthorizedUsersCanReceiveList(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            await DbSessionProvider.PerformCommitAsync();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetSharedUsersRequest()
            {
                Id = user.ApplicationId
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetSharedUser)]
        public async Task ShouldNotReceiveListIfUserHasReadOnlyAccess(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            await ApplicationService.ShareForUser(user1.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new GetSharedUsersRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetSharedUser)]
        public async Task ShouldNotReceiveList(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            await ApplicationService.ShareForUser(user1.Application, user2);
            var user3 = await DataSeeder.CreateActivatedUser();
            await ApplicationService.ShareForUser(user1.Application, user3);
            var user4 = await DataSeeder.CreateActivatedUser();
            await ApplicationService.ShareForUser(user1.Application, user4);

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetSharedUsersRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            response.EnsureSuccessStatusCode();

            var responseData = await response.GetJsonDataAsync<UsersListDto>();
            Assert.Equal(3, responseData.Count);

            Assert.Contains(responseData, i => i.Id == user2.Id);
            Assert.Contains(responseData, i => i.Id == user3.Id);
            Assert.Contains(responseData, i => i.Id == user4.Id);
        }
    }
}