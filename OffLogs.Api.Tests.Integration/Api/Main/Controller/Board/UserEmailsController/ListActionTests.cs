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
    public class ListActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/user/email/list";
        
        public ListActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task OnlyAuthorizedUsersCanDelete()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            var user1 = await DataSeeder.CreateActivatedUser();
            fakeRecord.SetUser(user1.Original);
            await CommandBuilder.SaveAsync(user1.Original);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new GetListRequest());
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task ShouldReceiveList()
        {
            var recordsCount = GlobalConstants.MaxUserEmailsCount - 3;
            
            var userModel = await DataSeeder.CreateActivatedUser();
            UserEmailEntity fakeRecord;
            var user = userModel.Original;
            for (int i = 1; i <= recordsCount; i++)
            {
                fakeRecord = DataFactory.UserEmailFactory().Generate();
                fakeRecord.SetUser(user); 
                await CommandBuilder.SaveAsync(user);    
            }

            // Act
            var response = await PostRequestAsync(Url, userModel.ApiToken, new GetListRequest());
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<UserEmailsListDto>();
            Assert.Equal(recordsCount, data.Count);
            Assert.All(data, item =>
            {
                Assert.True(item.Id > 0);
                Assert.NotEmpty(item.Email);
            });
        }
    }
}
