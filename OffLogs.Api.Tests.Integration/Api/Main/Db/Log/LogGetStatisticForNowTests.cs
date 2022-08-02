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
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Error, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Information, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Warning, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Fatal, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Debug, expectedCounter);
            await CommitDbChanges();

            var statisticList = await QueryBuilder.For<ICollection<LogStatisticForNowDto>>()
                .WithAsync(new GetByApplicationOrUserCriteria(userModel.Id));

            var actualGrouped = statisticList.GroupBy(item => item.LogLevel).ToList();

            var groupedLogStatistic = actualGrouped
                .Select(item => item.ToList())
                .ToList();
            Assert.Contains(actualGrouped, item => item.Key == LogLevel.Error);
            Assert.Contains(actualGrouped, item => item.Key == LogLevel.Warning);
            Assert.Contains(actualGrouped, item => item.Key == LogLevel.Fatal);
            Assert.Contains(actualGrouped, item => item.Key == LogLevel.Debug);
            Assert.Contains(actualGrouped, item => item.Key == LogLevel.Information);
            foreach (var logStatistic in groupedLogStatistic)
            {
                Assert.True(logStatistic.Sum(logItem => logItem.Count) >= expectedCounter);
            }
        }

        [Fact]
        public async Task ShouldReceiveLogStatisticForApplication()
        {
            var expectedCounter = 13;
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Error, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Information, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Warning, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Fatal, expectedCounter);
            await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Debug, expectedCounter);

            var statisticList = await QueryBuilder.For<ICollection<LogStatisticForNowDto>>()
                .WithAsync(new GetByApplicationOrUserCriteria(userModel.Id, application.Id));

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
