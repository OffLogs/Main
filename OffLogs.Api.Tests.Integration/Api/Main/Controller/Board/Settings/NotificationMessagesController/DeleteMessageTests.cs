using System.Net;
using System.Threading.Tasks;
using Bogus;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Settings.NotificationMessage;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Settings.NotificationMessagesController
{
    public partial class DeleteMessageTests : MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.NotificationMessageDelete;

        private readonly Faker<NotificationMessageEntity> _messageFactory;
        
        public DeleteMessageTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _messageFactory = DataFactory.NotificationMessageFactory();
        }

        [Fact]
        public async Task OnlyAuthorizedUsersCanDoIt()
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new IdRequest()
            {
                Id = 2,
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            var message = _messageFactory.Generate();
            var user = await DataSeeder.CreateActivatedUser();

            message.User = user;
            await CommandBuilder.SaveAsync(message);
            await DbSessionProvider.PerformCommitAsync();
            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new IdRequest()
            {
                Id = message.Id
            });
            // Assert
            response.EnsureSuccessStatusCode();

            DbSessionProvider.CurrentSession.Clear();
            message = await QueryBuilder.FindByIdAsync<NotificationMessageEntity>(message.Id);
            Assert.Null(message);
        }
        
        [Fact]
        public async Task OnlyOwnerCanDelete()
        {
            var message = _messageFactory.Generate();
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            message.User = user;
            await CommandBuilder.SaveAsync(message);
            await DbSessionProvider.PerformCommitAsync();
            // Act
            var response = await PostRequestAsync(Url, user2.ApiToken, new IdRequest()
            {
                Id = message.Id
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
