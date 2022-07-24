using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Extensions;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class AddActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/application/add";

        public AddActionTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
        }

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
        public async Task ShouldReturnErrorIfPackageRestrictionsHappens()
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            for (int i = 0; i < 4; i++)
            {
                var application = DataFactory.ApplicationFactory().Generate();
                await ApplicationService.CreateNewApplication(user1, application.Name);
            }
            
            // Act
            var response = await PostRequestAsync(Url, user1.ApiToken, new AddRequest()
            {
                Name = "SomeApp name"
            });
            // Assert
            var responseData = await response.GetJsonErrorAsync();
            Assert.Equal(new PaymentPackageRestrictionException().GetTypeName(), responseData.Type);
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
            Assert.Equal(user1.Id, responseData.UserId);
        }
    }
}
