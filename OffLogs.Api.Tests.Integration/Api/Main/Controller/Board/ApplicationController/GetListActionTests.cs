using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Controller.Board.Application.Actions;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class GetListActionTests: MyApiIntegrationTest
    {
        public GetListActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(MainApiUrl.ApplicationList)]
        public async Task OnlyAuthorizedUsersCanReceiveList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DbSessionProvider.PerformCommitAsync();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetListRequest()
            {
                Page = 1
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationList)]
        public async Task OnlyOwnerCanReceiveApplications(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Single(responseData.Items);
            
            foreach (var application in responseData.Items)
            {
                Assert.NotEqual(DateTime.MinValue, application.CreateTime);
                Assert.NotEmpty(application.Name);
                Assert.Equal(user1.Id, responseData.Items.First().UserId);
                Assert.True(application.Id > 0);
            }
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationList)]
        public async Task ShouldReceiveLogsList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user, 3);

            var user2 = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user2, 2);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(1, responseData.TotalPages);
            Assert.Equal(4, responseData.Items.Count);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationList)]
        public async Task ShouldReceiveMoreThanOnePages(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user, 25);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(2, responseData.TotalPages);
            Assert.Equal(20, responseData.Items.Count);
            Assert.Equal(26, responseData.TotalCount);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationList)]
        public async Task ApplicationListShouldContainSharedApplications(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user, 10);

            var user2 = await DataSeeder.CreateNewUser();
            foreach (var applicationOfUser2 in await DataSeeder.CreateApplicationsAsync(user2, 3))
            {
                await ApplicationService.ShareForUser(applicationOfUser2, user);
            }

            var user3 = await DataSeeder.CreateNewUser();
            foreach (var applicationOfUser3 in await DataSeeder.CreateApplicationsAsync(user3, 3))
            {
                await ApplicationService.ShareForUser(applicationOfUser3, user);
            }

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(1, responseData.TotalPages);
            Assert.Equal(17, responseData.Items.Count);
        }
    }
}