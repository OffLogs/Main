using System;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserController
{
    public class RegistrationStep2ActionsTests : MyApiIntegrationTest
    {
        const string Url = "/user/registration/step2";
        
        public RegistrationStep2ActionsTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData(Url)]
        public async Task ShouldFailIfIncorrectToken(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep2Request()
            {
                Token = "Fake token",
                Password = "password",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData(Url)]
        public async Task ShouldFailIfUserIsActivated(string url)
        {
            var fakeUser = DataFactory.UserFactory().Generate();
            // Arrange
            var user = await UserService.CreatePendingUser(fakeUser.Email);
            var (_, _) = await UserService.ActivateUser(
                user.Id, 
                SecurityUtil.GeneratePassword()
            );
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep2Request
            {
                Token = user.VerificationToken,
                Password = "password",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData(Url)]
        public async Task ShouldActivateUser(string url)
        {
            // Arrange
            var password = SecurityUtil.GeneratePassword();
            var fakeUser = DataFactory.UserFactory().Generate();
            // Arrange
            var user = await UserService.CreatePendingUser(fakeUser.Email);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new RegistrationStep2Request()
            {
                Token = user.VerificationToken,
                Password = password
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<RegistrationStep2ResponseDto>();
            Assert.True(JwtAuthService.IsValidJwt(responseData.JwtToken));
            var encryptor = AsymmetricEncryptor.ReadFromPem(responseData.Pem, password);
            Assert.NotNull(encryptor.PrivateKey);
            Assert.NotNull(encryptor.PublicKey);
            
            AsymmetricEncryptor.FromPrivateKeyBytes(
                Convert.FromBase64String(responseData.PrivateKeyBase64)    
            );
        }
    }
}
