using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.Log
{
    [Collection("LogDaoTest.LogListTests")]
    public class LogGetStatisticForNowTests : MyApiIntegrationTest
    {
        public LogGetStatisticForNowTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldReceiveLogStatisticWithoutApplication()
        {
            var expectedCounter = 15;
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Error, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Information, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Warning, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Fatal, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Debug, expectedCounter);

            var statisticList = await QueryBuilder.For<ICollection<LogStatisticForNowDto>>()
                .WithAsync(new LogGetStatisticForNowCriteria());

            Assert.Contains(statisticList, item => item.LogLevel == LogLevel.Error && item.Count >= expectedCounter);
            Assert.Contains(statisticList, item => item.LogLevel == LogLevel.Information && item.Count >= expectedCounter);
            Assert.Contains(statisticList, item => item.LogLevel == LogLevel.Warning && item.Count >= expectedCounter);
            Assert.Contains(statisticList, item => item.LogLevel == LogLevel.Fatal && item.Count >= expectedCounter);
            Assert.Contains(statisticList, item => item.LogLevel == LogLevel.Debug && item.Count >= expectedCounter);
        }

        [Fact]
        public async Task ShouldReceiveLogStatisticForApplication()
        {
            var expectedCounter = 13;
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Error, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Information, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Warning, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Fatal, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Debug, expectedCounter);

            var statisticList = await QueryBuilder.For<ICollection<LogStatisticForNowDto>>()
                .WithAsync(new LogGetStatisticForNowCriteria(application.Id));

            Assert.Equal(
                expectedCounter,
                statisticList.Where(item => item.LogLevel == LogLevel.Error).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                statisticList.Where(item => item.LogLevel == LogLevel.Information).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                statisticList.Where(item => item.LogLevel == LogLevel.Warning).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                statisticList.Where(item => item.LogLevel == LogLevel.Fatal).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                statisticList.Where(item => item.LogLevel == LogLevel.Debug).Sum(item => item.Count)
            );
        }
    }
}
