using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserController
{
    public class RegistrationStep1ActionsTests : MyApiIntegrationTest
    {
        const string Url = "/user/registration/step1";
        
        public RegistrationStep1ActionsTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData(Url)]
        public async Task ShouldFailIfIncorrectEmail(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep1Request()
            {
                Email = "bad email",
                ReCaptcha = "testRecaptcha",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData(Url)]
        public async Task ShouldNotRegisterIfExists(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep1Request()
            {
                Email = user.Email.ToUpper(),
                ReCaptcha = "testRecaptcha",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData(Url)]
        public async Task ShouldAddPendingUserAndSendNotification(string url)
        {
            var newUser = DataFactory.UserFactory().Generate();
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep1Request
            {
                Email = newUser.Email,
                ReCaptcha = "testRecaptcha",
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);
        }
        
        [Theory]
        [InlineData(Url)]
        public async Task ShouldReSendNotificationForPendingUser(string url)
        {
            var newUser = DataFactory.UserFactory().Generate();
            // Arrange
            var user = await UserService.CreatePendingUser(newUser.Email);

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep1Request
            {
                Email = newUser.Email,
                ReCaptcha = "testRecaptcha",
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);
        }
    }
}
