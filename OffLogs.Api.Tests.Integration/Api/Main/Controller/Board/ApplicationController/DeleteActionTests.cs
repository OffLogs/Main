using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class DeleteActionTests : MyApiIntegrationTest
    {
        private const string Url = "/board/application/delete";
        
        public DeleteActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(Url)]
        public async Task OnlyAuthorizedUsersCanUpdateApplications(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new DeleteRequest()
            {
                Id = 1
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(Url)]
        public async Task CanNotDeleteForOtherUser(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new DeleteRequest()
            {
                Id = user2.ApplicationId
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(Url)]
        public async Task CanDeleteApplication(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new DeleteRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            response.EnsureSuccessStatusCode();

            await DbSessionProvider.PerformCommitAsync();
            DbSessionProvider.CurrentSession.Clear();

            var actualApplication = await QueryBuilder.FindByIdAsync<ApplicationEntity>(user1.ApplicationId);
            Assert.Null(actualApplication);
        }

        [Theory]
        [InlineData(Url)]
        public async Task CanDeleteApplicationAndSendNotifications(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();
            var user3 = await DataSeeder.CreateNewUser();
            await ApplicationService.ShareForUser(user1.Application, user2);
            await ApplicationService.ShareForUser(user1.Application, user3);
            await DbSessionProvider.PerformCommitAsync();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new DeleteRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            response.EnsureSuccessStatusCode();

            await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);

            Assert.Contains(EmailSendingService.SentMessages, m => m.To == user1.Email);
            Assert.Contains(EmailSendingService.SentMessages, m => m.To == user2.Email);
            Assert.Contains(EmailSendingService.SentMessages, m => m.To == user3.Email);
        }

        [Theory]
        [InlineData(Url)]
        public async Task SharedUserShouldNotDeleteApplication(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            await ApplicationService.ShareForUser(user1.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new DeleteRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}