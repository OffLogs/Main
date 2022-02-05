using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class AddActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/application/add";
        
        public AddActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task OnlyAuthorizedUsersCanAddApplications()
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new AddRequest()
            {
                Name = "aaa"
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldAddApplication()
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(Url, user1.ApiToken, new AddRequest()
            {
                Name = "SomeApp name"
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.True(responseData.Id > 0);
            Assert.NotEmpty(responseData.Name);
            Assert.NotEmpty(responseData.PublicKeyBase64);
            Assert.NotEmpty(responseData.EncryptedPrivateKeyBase64);
            Assert.Equal(user1.Id, responseData.UserId);
        }
    }
}
