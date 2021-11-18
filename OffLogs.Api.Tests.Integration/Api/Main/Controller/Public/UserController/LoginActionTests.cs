using System;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserController
{
    public class LoginActionTests : MyApiIntegrationTest
    {
        private const string Url = "/user/login";
        
        public LoginActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldNotLoginIfBase64IsIncorrect()
        {
            // Arrange
            var password = SecurityUtil.GeneratePassword();
            var dataToSign = SecurityUtil.GetRandomString(16);
            var fakeUser = DataFactory.UserFactory().Generate();
            // Arrange
            var pendingUser = await UserService.CreatePendingUser(fakeUser.Email);
            var (user , pem) = await UserService.ActivateUser(pendingUser.Id, password);

            var encryptor = AsymmetricEncryptor.ReadFromPem(pem, password);
            var signData = encryptor.SignData(dataToSign);
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new LoginRequest
            {
                PublicKeyBase64 = "Fake key",
                SignBase64 = Convert.ToBase64String(signData),
                SignedData = dataToSign
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ShouldLoginIfNotExists()
        {
            // Arrange
            var password = SecurityUtil.GeneratePassword();
            var dataToSign = SecurityUtil.GetRandomString(16);
            var fakeUser = DataFactory.UserFactory().Generate();
            // Arrange
            var pendingUser = await UserService.CreatePendingUser(fakeUser.Email);
            var (user , pem) = await UserService.ActivateUser(pendingUser.Id, password);

            var encryptorWithAnotherPublic = AsymmetricEncryptor.GenerateKeyPair();
            
            var encryptor = AsymmetricEncryptor.ReadFromPem(pem, password);
            var signData = encryptor.SignData(dataToSign);
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new LoginRequest
            {
                PublicKeyBase64 = Convert.ToBase64String(encryptorWithAnotherPublic.GetPublicKeyBytes()),
                SignBase64 = Convert.ToBase64String(signData),
                SignedData = dataToSign
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ShouldLogin()
        {
            // Arrange
            var password = SecurityUtil.GeneratePassword();
            var dataToSign = SecurityUtil.GetRandomString(16);
            var fakeUser = DataFactory.UserFactory().Generate();
            // Arrange
            var pendingUser = await UserService.CreatePendingUser(fakeUser.Email);
            var (user , pem) = await UserService.ActivateUser(pendingUser.Id, password);
            
            var actualEncryptor = AsymmetricEncryptor.ReadFromPem(pem, password);
            var actualSign = actualEncryptor.SignData(dataToSign);
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new LoginRequest
            {
                PublicKeyBase64 = Convert.ToBase64String(actualEncryptor.GetPublicKeyBytes()),
                SignBase64 = Convert.ToBase64String(actualSign),
                SignedData = dataToSign
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<LoginResponseDto>();
            Assert.NotEmpty(responseData.Token);
            JwtAuthService.IsValidJwt(responseData.Token);
        }
    }
}
