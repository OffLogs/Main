using System;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserEmailController;

public class VerifyActionTests : MyApiIntegrationTest
{
    private const string Url = "/user/email/verify/";
        
    public VerifyActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task ShouldVerify()
    {
        var userEmail = DataFactory.UserEmailFactory().Generate();
        var userModel = await DataSeeder.CreateActivatedUser();
        var user = userModel.Original;
        userEmail = await UserEmailService.Add(user, userEmail.Email);
        
        // Act
        var response = await GetRequestAsAnonymousAsync(Url + userEmail.VerificationToken);
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task ShouldNotVerifyIfIncorrectToken()
    {
        var userEmail = DataFactory.UserEmailFactory().Generate();
        var userModel = await DataSeeder.CreateActivatedUser();
        var user = userModel.Original;
        userEmail = await UserEmailService.Add(user, userEmail.Email);
        
        // Act
        var response = await GetRequestAsAnonymousAsync(Url + "fakeToken");
        // Assert
        var data = await response.GetJsonErrorAsync();
        Assert.Equal(new RecordNotFoundException().GetTypeName(), data.Type);
    }
}
