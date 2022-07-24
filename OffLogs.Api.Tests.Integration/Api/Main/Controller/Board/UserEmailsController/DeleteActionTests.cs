using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.UserEmailsController
{
    public class DeleteActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/user/email/delete";
        
        public DeleteActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task OnlyAuthorizedUsersCanDelete()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            var user1 = await DataSeeder.CreateActivatedUser();
            fakeRecord.SetUser(user1.Original);
            await CommandBuilder.SaveAsync(user1.Original);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new DeleteRequest()
            {
                Id = fakeRecord.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            var userModel = await DataSeeder.CreateActivatedUser();
            var user = userModel.Original;
            fakeRecord.SetUser(user); 
            await CommandBuilder.SaveAsync(user);
            await CommitDbChanges();

            // Act
            var response = await PostRequestAsync(Url, userModel.ApiToken, new DeleteRequest()
            {
                Id = fakeRecord.Id
            });
            // Assert
            response.EnsureSuccessStatusCode();

            DbSessionProvider.CurrentSession.Clear();
            user = await QueryBuilder.FindByIdAsync<UserEntity>(user.Id);
            
            Assert.Equal(0, user.Emails.Count);
        }
        
        [Fact]
        public async Task ShouldSendErrorIfNotFound()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            var userModel = await DataSeeder.CreateActivatedUser();
            var user = userModel.Original;
            fakeRecord.SetUser(user); 
            await CommandBuilder.SaveAsync(user);
            await CommitDbChanges();

            // Act
            var response = await PostRequestAsync(Url, userModel.ApiToken, new DeleteRequest()
            {
                Id = 333333
            });
            // Assert
            var data = await response.GetJsonErrorAsync();
            Assert.Equal(new RecordNotFoundException().GetTypeName(), data.Type);
        }
    }
}
