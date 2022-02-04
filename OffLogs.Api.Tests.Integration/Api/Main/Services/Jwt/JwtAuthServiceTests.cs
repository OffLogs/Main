using System;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Entities;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Jwt
{
    public class JwtAuthServiceTests : MyApiIntegrationTest
    {
        public JwtAuthServiceTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldGenerateAndValidateJwtToken()
        {
            var expectedUserId = 2;
            
            var jwtToken = JwtAuthService.BuildJwt(expectedUserId);

            var isValid = JwtAuthService.IsValidJwt(jwtToken);
            Assert.True(isValid);
            Assert.Equal(expectedUserId, JwtAuthService.GetUserId(jwtToken));
        }
    }
}
