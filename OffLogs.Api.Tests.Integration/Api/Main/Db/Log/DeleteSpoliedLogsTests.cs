using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Commands.Entities.Log;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.Log
{
    public class DeleteSpoliedLogsTests : MyApiIntegrationTest
    {
        public DeleteSpoliedLogsTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldDeleteOneLog()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log1 = await CreateLog(application, LogLevel.Error);
            log1.CreateTime = DateTime.UtcNow.AddDays(-31);
            await CommandBuilder.SaveAsync(log1);
            await LogShareService.Share(log1);

            await DbSessionProvider.PerformCommitAsync();

            await CommandBuilder.ExecuteAsync(new LogDeleteSpoiledCommandContext());
            await DbSessionProvider.PerformCommitAsync();

            // Clear session
            DbSessionProvider.CurrentSession.Clear();

            var actualLog1 = await QueryBuilder.FindByIdAsync<LogEntity>(log1.Id);
            Assert.Null(actualLog1);
        }

        [Fact]
        public async Task ShouldDeleteSpoiledLogs()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log1 = await CreateLog(application, LogLevel.Error);
            log1.CreateTime = DateTime.UtcNow.AddDays(-31);
            await CommandBuilder.SaveAsync(log1);
            await LogShareService.Share(log1);

            var log2 = await CreateLog(application, LogLevel.Error);
            log2.CreateTime = DateTime.UtcNow.AddDays(-31);
            await CommandBuilder.SaveAsync(log2);
            await LogShareService.Share(log2);

            var log3 = await CreateLog(application, LogLevel.Error);
            log3.CreateTime = DateTime.UtcNow.AddDays(-29);
            await CommandBuilder.SaveAsync(log3);
            await LogShareService.Share(log3);
            await LogService.SetIsFavoriteAsync(userModel.Id, log3.Id, true);
            await DbSessionProvider.PerformCommitAsync();

            await CommandBuilder.ExecuteAsync(new LogDeleteSpoiledCommandContext());
            await DbSessionProvider.PerformCommitAsync();
            
            // Clear session
            DbSessionProvider.CurrentSession.Clear();

            var actualLog1 = await QueryBuilder.FindByIdAsync<LogEntity>(log1.Id);
            var actualLog2 = await QueryBuilder.FindByIdAsync<LogEntity>(log2.Id);
            var actualLog3 = await QueryBuilder.FindByIdAsync<LogEntity>(log3.Id);

            Assert.Null(actualLog1);
            Assert.Null(actualLog2);
            Assert.NotNull(actualLog3);
        }

        [Fact]
        public async Task ShouldNotDeleteFavoriteLogs()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log1 = await CreateLog(application, LogLevel.Error);
            log1.CreateTime = DateTime.UtcNow.AddDays(-31);
            await CommandBuilder.SaveAsync(log1);
            await LogService.SetIsFavoriteAsync(userModel.Id, log1.Id, true);

            await DbSessionProvider.PerformCommitAsync();
            await CommandBuilder.ExecuteAsync(new LogDeleteSpoiledCommandContext());

            // Clear session
            DbSessionProvider.CurrentSession.Clear();

            var actualLog1 = await QueryBuilder.FindByIdAsync<LogEntity>(log1.Id);

            Assert.NotNull(actualLog1);
        }

        private async Task<LogEntity> CreateLog(ApplicationEntity application, LogLevel level)
        {
            return await LogService.AddAsync(
                application,
                "SomeMessage",
                level,
                DateTime.UtcNow,
                new List<LogPropertyEntity>()
                {
                    new()
                    {
                        Key = "TEST_PROP",
                        Value = "TEST_VALUE",
                    }
                },
                new List<LogTraceEntity>()
                {
                    new()
                    {
                        Trace = "TestTrace",
                        CreateTime = DateTime.UtcNow
                    }
                }
            );
        }
    }
}
