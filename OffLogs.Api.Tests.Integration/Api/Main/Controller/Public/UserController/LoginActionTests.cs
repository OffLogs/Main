using System;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserController;

public class LoginActionTests : MyApiIntegrationTest
{
    private const string Url = "/user/login";
        
    public LoginActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task ShouldNotLoginIfPasswordIncorrect()
    {
        // Arrange
        var password = SecurityUtil.GeneratePassword();
        var fakeUser = DataFactory.UserFactory().Generate();
        // Arrange
        var pendingUser = await UserService.CreatePendingUser(fakeUser.Email);
        var (user , pem) = await UserService.ActivateUser(pendingUser.Id, password);

        var encryptor = AsymmetricEncryptor.ReadFromPem(pem, password);
            
        // Act
        var response = await PostRequestAsAnonymousAsync(Url, new LoginRequest
        {
            Password = "Fake password",
            Pem = encryptor.CreatePem(password),
            ReCaptcha = "fake"
        });
        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldLoginIfNotExists()
    {
        // Arrange
        var password = SecurityUtil.GeneratePassword();
        var fakeUser = DataFactory.UserFactory().Generate();
        // Arrange
        var pendingUser = await UserService.CreatePendingUser(fakeUser.Email);
        var (user , pem) = await UserService.ActivateUser(pendingUser.Id, password);

        var encryptorWithAnotherPublic = AsymmetricEncryptor.GenerateKeyPair();

        // Act
        var response = await PostRequestAsAnonymousAsync(Url, new LoginRequest
        {
            Password = password,
            Pem = encryptorWithAnotherPublic.CreatePem(password),
            ReCaptcha = "fake"
        });
        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldLogin()
    {
        // Arrange
        var password = SecurityUtil.GeneratePassword();
        var fakeUser = DataFactory.UserFactory().Generate();
        // Arrange
        var pendingUser = await UserService.CreatePendingUser(fakeUser.Email);
        var (user , pem) = await UserService.ActivateUser(pendingUser.Id, password);
            
        var actualEncryptor = AsymmetricEncryptor.ReadFromPem(pem, password);
        // Act
        var response = await PostRequestAsAnonymousAsync(Url, new LoginRequest
        {
            Password = password,
            Pem = actualEncryptor.CreatePem(password),
            ReCaptcha = "fake"
        });
        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.GetJsonDataAsync<LoginResponseDto>();
        Assert.NotEmpty(responseData.Token);
        JwtAuthService.IsValidJwt(responseData.Token);

        AsymmetricEncryptor.FromPrivateKeyBytes(
            Convert.FromBase64String(responseData.PrivateKeyBase64)    
        );
    }
}
