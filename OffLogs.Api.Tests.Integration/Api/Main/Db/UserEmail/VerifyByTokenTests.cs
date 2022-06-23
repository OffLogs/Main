using System;
using System.Threading.Tasks;
using OffLogs.Business.Common.Exceptions.Api;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.UserEmail
{
    public class VerifyByTokenTests : MyApiIntegrationTest
    {
        public VerifyByTokenTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldThrowExceptionIfNotFound()
        {
            var userEmail = DataFactory.UserEmailFactory().Generate();
            var expectedUser = await DataSeeder.CreateActivatedUser();
            await UserEmailService.Add(expectedUser.Original, userEmail.Email);
            Assert.False(expectedUser.IsVerified);

            await Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await UserEmailService.VerifyByToken("Fake token");
            });
        }
        
        [Fact]
        public async Task ShouldVerify()
        {
            var userEmail = DataFactory.UserEmailFactory().Generate();
            var expectedUser = await DataSeeder.CreateActivatedUser();
            userEmail = await UserEmailService.Add(expectedUser.Original, userEmail.Email);
            Assert.False(expectedUser.IsVerified);

            userEmail = await UserEmailService.VerifyByToken(userEmail.VerificationToken);
            
            Assert.True(userEmail.Id > 0);
            Assert.NotNull(userEmail.VerificationTime);
            Assert.Null(userEmail.VerificationToken);
        }
    }
}
