using System.Net;
using System.Threading.Tasks;
using Bogus;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Notifications.MessageTemplatesController
{
    public partial class SetMessageTests : MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.NotificationMessageTemplateSet;

        private readonly Faker<MessageTemplateEntity> _messageFactory;
        
        public SetMessageTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _messageFactory = DataFactory.NotificationMessageFactory();
        }

        [Fact]
        public async Task OnlyAuthorizedUsersCanDoIt()
        {
            var expectedMessage = _messageFactory.Generate();

            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new SetMessageTemplateRequest()
            {
                Subject = expectedMessage.Subject,
                Body = expectedMessage.Body
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldAddNew()
        {
            var expectedMessage = _messageFactory.Generate();
            var user = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new SetMessageTemplateRequest()
            {
                Subject = expectedMessage.Subject,
                Body = expectedMessage.Body
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<MessageTemplateDto>();
            Assert.True(data.Id > 0);
            Assert.Equal(expectedMessage.Subject, data.Subject);
            Assert.Equal(expectedMessage.Body, data.Body);
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            var oldMessage = _messageFactory.Generate();
            var expectedMessage = _messageFactory.Generate();
            var user = await DataSeeder.CreateActivatedUser();

            oldMessage.User = user;
            await CommandBuilder.SaveAsync(oldMessage);
            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new SetMessageTemplateRequest()
            {
                Id = oldMessage.Id,
                Subject = expectedMessage.Subject,
                Body = expectedMessage.Body
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<MessageTemplateDto>();
            Assert.Equal(oldMessage.Id, data.Id);
            Assert.Equal(expectedMessage.Subject, data.Subject);
            Assert.Equal(expectedMessage.Body, data.Body);
        }
        
        [Fact]
        public async Task OnlyOwnerCanUpdate()
        {
            var oldMessage = _messageFactory.Generate();
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            oldMessage.User = user;
            await CommandBuilder.SaveAsync(oldMessage);
            // Act
            var response = await PostRequestAsync(Url, user2.ApiToken, new SetMessageTemplateRequest()
            {
                Id = oldMessage.Id,
                Subject = oldMessage.Subject,
                Body = oldMessage.Body
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
