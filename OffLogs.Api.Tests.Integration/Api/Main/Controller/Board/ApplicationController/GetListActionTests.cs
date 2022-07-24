using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class GetListActionTests: MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.ApplicationList;
        
        public GetListActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        #region Common
        [Fact]
        public async Task OnlyAuthorizedUsersCanReceiveList()
        {
            var user = await DataSeeder.CreateActivatedUser();
            await DbSessionProvider.PerformCommitAsync();

            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new GetListRequest()
            {
                Page = 1
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task OnlyOwnerCanReceiveApplications()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(Url, user1.ApiToken, new GetListRequest()
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

        [Fact]
        public async Task ShouldReceiveLogsList()
        {
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateApplicationsAsync(user, 3);

            var user2 = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateApplicationsAsync(user2, 2);

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(1, responseData.TotalPages);
            Assert.Equal(4, responseData.Items.Count);
        }

        [Fact]
        public async Task ShouldReceiveMoreThanOnePages()
        {
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateApplicationsAsync(user, 25);

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            // response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(2, responseData.TotalPages);
            Assert.Equal(20, responseData.Items.Count);
            Assert.Equal(26, responseData.TotalCount);
        }

        [Fact]
        public async Task ApplicationListShouldContainSharedApplications()
        {
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateApplicationsAsync(user, 10);

            var user2 = await DataSeeder.CreateActivatedUser();
            foreach (var applicationOfUser2 in await DataSeeder.CreateApplicationsAsync(user2, 3))
            {
                await ApplicationService.ShareForUser(applicationOfUser2, user);
            }

            var user3 = await DataSeeder.CreateActivatedUser();
            foreach (var applicationOfUser3 in await DataSeeder.CreateApplicationsAsync(user3, 3))
            {
                await ApplicationService.ShareForUser(applicationOfUser3, user);
            }

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(1, responseData.TotalPages);
            Assert.Equal(19, responseData.Items.Count);
        }
        #endregion
        
        #region Permissions
        [Fact]
        public async Task ListShouldContainSharedApplicationsWithCorrectPermissions()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            await ApplicationService.ShareForUser(user2.Application, user);
            var user3 = await DataSeeder.CreateActivatedUser();
            await ApplicationService.ShareForUser(user3.Application, user);
            
            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetListRequest()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<ApplicationListItemDto>>();
            Assert.Equal(3, responseData.Items.Count);
            Assert.Contains(
                responseData.Items, 
                dto => dto.Id == user.ApplicationId 
                    && dto.Permissions.IsHasReadAccess 
                    && dto.Permissions.IsHasWriteAccess
            );
            Assert.Contains(
                responseData.Items, 
                dto => dto.Id == user2.ApplicationId
                       && dto.Permissions.IsHasReadAccess 
                       && !dto.Permissions.IsHasWriteAccess
            );
            Assert.Contains(
                responseData.Items, 
                dto => dto.Id == user3.ApplicationId
                       && dto.Permissions.IsHasReadAccess 
                       && !dto.Permissions.IsHasWriteAccess
            );
        }
        #endregion
    }
}
