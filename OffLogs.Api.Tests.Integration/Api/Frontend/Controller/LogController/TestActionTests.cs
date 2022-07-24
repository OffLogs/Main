using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Monetization;
using OffLogs.Business.Services.Redis.Clients;
using Serilog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Frontend.Controller.LogController
{
    public class TestActionTests: MyApiFrontendIntegrationTest
    {
        private const string Url = "/log/add/test";

        private readonly IPaymentPackageService _paymentPackageService;
        private readonly IUserInfoRedisClient _userInfoRedisClient;

        public TestActionTests(ApiFrontendCustomWebApplicationFactory factory) : base(factory)
        {
            _paymentPackageService = _factory.Services.GetRequiredService<IPaymentPackageService>();
            _userInfoRedisClient = _factory.Services.GetRequiredService<IUserInfoRedisClient>();
        }

        [Fact]
        public async Task ShouldThrowExceptionIfTooManyRequests()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            // Act
            var logs = new List<object>()
            {
                new
                {
                    Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                    Level = SiteLogLevel.Information.GetValue(),
                    Message = "This is Information message"
                }
            };

            HttpResponseMessage response;
            response = await PostRequestAsync(Url, user.ApplicationApiToken, new { logs });
            response.EnsureSuccessStatusCode();

            for (int i = 0; i < 50; i++)
            {
                await ThrottleRequestsService.CheckOrThrowExceptionByApplicationIdAsync(
                    user.ApplicationId,
                    user.Id
                );
            }

            response = await PostRequestAsync(Url, user.ApplicationApiToken, new { logs });
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
