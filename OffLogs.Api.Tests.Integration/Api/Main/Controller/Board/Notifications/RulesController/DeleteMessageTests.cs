using System.Net;
using System.Threading.Tasks;
using Bogus;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Notifications.RulesController
{
    public partial class DeleteMessageTests : MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.NotificationRuleDelete;

        private readonly Faker<NotificationRuleEntity> _ruleFactory;
        private readonly Faker<MessageTemplateEntity> _templateFactory;
        
        public DeleteMessageTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _ruleFactory = DataFactory.NotificationRuleFactory();
            _templateFactory = DataFactory.MessageTemplateFactory();
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
            var user = await DataSeeder.CreateActivatedUser();
            var rule = await CreateRule(user);

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new DeleteRuleRequest()
            {
                Id = rule.Id
            });
            // Assert
            response.EnsureSuccessStatusCode();

            DbSessionProvider.CurrentSession.Clear();
            rule = await QueryBuilder.FindByIdAsync<NotificationRuleEntity>(rule.Id);
            Assert.Null(rule);
        }
        
        [Fact]
        public async Task OnlyOwnerCanDelete()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var rule = await CreateRule(user);
            var user2 = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsync(Url, user2.ApiToken, new DeleteRuleRequest()
            {
                Id = rule.Id
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private async Task<NotificationRuleEntity> CreateRule(UserTestModel user)
        {
            var rule = _ruleFactory.Generate();
            rule.MessageTemplate = _templateFactory.Generate();
            rule.MessageTemplate.User = user;
            rule.Application = user.Application;
            rule.User = user;
            await CommandBuilder.SaveAsync(rule);
            await DbSessionProvider.PerformCommitAsync();
            return rule;
        }
    }
}
