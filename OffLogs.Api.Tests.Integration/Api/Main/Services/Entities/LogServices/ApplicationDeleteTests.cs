using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Entities.LogServices
{
    public class LogAssemblerTests : MyApiIntegrationTest
    {
        public LogAssemblerTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldAssembleAndDecryptLogData()
        {
            var expectedMessage = SecurityUtil.GetRandomString(32);
            var expectedPropertyKey = SecurityUtil.GetRandomString(32);
            var expectedPropertyValue = SecurityUtil.GetRandomString(32);
            var expectedTraceValue = SecurityUtil.GetRandomString(32);
            
            var user = await DataSeeder.CreateActivatedUser();
            
            var log = await LogService.AddAsync(
                user.Application,
                expectedMessage,
                LogLevel.Warning,
                DateTime.UtcNow,
                new Dictionary<string, object>()
                {
                    { expectedPropertyKey, expectedPropertyValue }
                },
                new List<string> { expectedTraceValue }
            );

            var encryptor = AsymmetricEncryptor.ReadFromPem(user.PemFile, user.PemFilePassword);
            var actualLog = await LogAssembler.AssembleDecryptedLogAsync(log, encryptor.GetPrivateKeyBytes());
            
            Assert.Equal(expectedMessage, actualLog.Message);
            Assert.Equal(expectedPropertyKey, actualLog.Properties.First().Key);
            Assert.Contains(expectedPropertyValue, actualLog.Properties.First().Value);
            Assert.Equal(expectedTraceValue, actualLog.Traces.First().Trace);
        }
    }
}
