using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Controller.Board.User.Actions;
using OffLogs.Api.Controller.Board.User.Dto;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.UserController
{
    public class SearchActionTests : MyApiIntegrationTest
    {
        public SearchActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData(MainApiUrl.UserSearch)]
        public async Task OnlyAuthorizedUsersCanReceiveList(string url)
        {
            var user = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new SearchRequest()
            {
                Search = "123"
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.UserSearch)]
        public async Task ShouldReceiveUsersByUserName(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new SearchRequest()
            {
                Search = user2.UserName.Substring(1, user2.UserName.Length - 2)
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<SearchResponseDto>();
            Assert.True(responseData.Count >= 1);
            Assert.Contains(responseData, u => u.Id == user2.Id);
        }

        [Theory]
        [InlineData(MainApiUrl.UserSearch)]
        public async Task ShouldReceiveUsersByEmail(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new SearchRequest()
            {
                Search = user2.Email.Substring(1, user2.Email.Length - 2)
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<SearchResponseDto>();
            Assert.True(responseData.Count >= 1);
            Assert.Contains(responseData, u => u.Id == user2.Id);
        }

        [Theory]
        [InlineData(MainApiUrl.UserSearch)]
        public async Task ShouldReceiveUsersButNotCurrentByUserName(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new SearchRequest()
            {
                Search = user1.UserName
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<SearchResponseDto>();
            Assert.True(responseData.Count >= 0);
            Assert.DoesNotContain(responseData, u => u.Id == user1.Id);
        }

        [Theory]
        [InlineData(MainApiUrl.UserSearch)]
        public async Task ShouldReceiveUsersButNotCurrentByEmail(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new SearchRequest()
            {
                Search = user1.Email
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<SearchResponseDto>();
            Assert.True(responseData.Count >= 0);
            Assert.DoesNotContain(responseData, u => u.Id == user1.Id);
        }
    }
}