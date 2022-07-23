using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Http.ThrottleRequests;
using Serilog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Frontend.Controller.LogController
{
    public class TestActionTests: MyApiFrontendIntegrationTest
    {
        private const string Url = "/log/add/test";
        
        public TestActionTests(ApiFrontendCustomWebApplicationFactory factory) : base(factory) {}

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

            for (int i = 0; i < 500; i++)
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
