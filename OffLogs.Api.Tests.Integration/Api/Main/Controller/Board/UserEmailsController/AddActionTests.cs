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
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.UserEmailsController
{
    public class AddActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/user/email/add";
        
        public AddActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task OnlyAuthorizedUsersCanAddApplications()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new AddRequest()
            {
                Email = fakeRecord.Email
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldAdd()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            var user1 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(Url, user1.ApiToken, new AddRequest()
            {
                Email = fakeRecord.Email.ToUpperFirstChar()
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<UserEmailDto>();
            Assert.True(responseData.Id > 0);
            Assert.Equal(fakeRecord.Email.ToLower(), responseData.Email);
        }
        
        [Fact]
        public async Task CanNotAddExistEmail()
        {
            var fakeRecord = DataFactory.UserEmailFactory().Generate();
            var user1 = await DataSeeder.CreateActivatedUser();
            fakeRecord.SetUser(user1.Original); 
            await CommandBuilder.SaveAsync(user1.Original);
            
            // Act
            var response = await PostRequestAsync(Url, user1.ApiToken, new AddRequest()
            {
                Email = fakeRecord.Email
            });
            // Assert
            var responseData = await response.GetJsonErrorAsync();
            Assert.Equal(new RecordIsExistsException().GetTypeName(), responseData.Type);
        }
        
        [Fact]
        public async Task ShouldReturnErrorIfPackageRestrictions()
        {
            UserEmailEntity fakeRecord;
            var user1 = await DataSeeder.CreateActivatedUser();
            for (int i = 0; i <= 5 + 1; i++)
            {
                fakeRecord = DataFactory.UserEmailFactory().Generate();
                fakeRecord.SetUser(user1.Original); 
                await CommandBuilder.SaveAsync(user1.Original);    
            }
            
            fakeRecord = DataFactory.UserEmailFactory().Generate();
            // Act
            var response = await PostRequestAsync(Url, user1.ApiToken, new AddRequest()
            {
                Email = fakeRecord.Email
            });
            // Assert
            var responseData = await response.GetJsonErrorAsync();
            Assert.Equal(new PaymentPackageRestrictionException().GetTypeName(), responseData.Type);
        }
    }
}
