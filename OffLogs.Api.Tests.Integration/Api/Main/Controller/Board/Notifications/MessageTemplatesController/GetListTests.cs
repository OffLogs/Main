using System.Net;
using System.Threading.Tasks;
using Bogus;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Notifications.MessageTemplatesController
{
    public class GetListTests : MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.NotificationMessageTemplateList;

        private readonly Faker<MessageTemplateEntity> _messageFactory;
        
        public GetListTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _messageFactory = DataFactory.MessageTemplateFactory();
        }

        [Fact]
        public async Task OnlyAuthorizedUsersCanDoIt()
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(Url);
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldGetList()
        {
            var expectCount = 5;
            var user = await DataSeeder.CreateActivatedUser();

            for (int i = 1; i <= expectCount; i++)
            {
                var expectedMessage = _messageFactory.Generate();
                expectedMessage.User = user;
                await CommandBuilder.SaveAsync(expectedMessage);
            }

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken);
            // Assert
            // response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<ListDto<MessageTemplateDto>>();
            Assert.Equal(expectCount, data.TotalCount);
            Assert.Equal(expectCount, data.Items.Count);
            Assert.NotEmpty(data.Items);
        }
        
        [Fact]
        public async Task ShouldGetOnlyForCurrentUser()
        {
            var expectCount = 7;
            var user = await DataSeeder.CreateActivatedUser();

            for (int i = 1; i <= expectCount; i++)
            {
                var expectedMessage = _messageFactory.Generate();
                expectedMessage.User = user;
                await CommandBuilder.SaveAsync(expectedMessage);
            }

            var user2 = await DataSeeder.CreateActivatedUser();
            for (int i = 1; i <= expectCount; i++)
            {
                var expectedMessage = _messageFactory.Generate();
                expectedMessage.User = user2;
                await CommandBuilder.SaveAsync(expectedMessage);
            }
            
            // Act
            var response = await PostRequestAsync(Url, user.ApiToken);
            // Assert
            // response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<ListDto<MessageTemplateDto>>();
            Assert.Equal(expectCount, data.TotalCount);
            Assert.Equal(expectCount, data.Items.Count);
            Assert.NotEmpty(data.Items);
        }
    }
}
