using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Commands.Entities.Application;
using OffLogs.Business.Orm.Commands.Entities.Log;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.Application
{
    public class DeleteApplicationTests : MyApiIntegrationTest
    {
        public DeleteApplicationTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldDeleteOne()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            var application = user.Applications.First();
            await ApplicationService.ShareForUser(application, user2);

            var log1 = await CreateLog(application, LogLevel.Error);
            await DbSessionProvider.PerformCommitAsync();

            await CommandBuilder.ExecuteAsync(new ApplicationDeleteCommandContext(application.Id));
            await DbSessionProvider.PerformCommitAsync();

            // Clear session
            DbSessionProvider.CurrentSession.Clear();

            var actualApplication = await QueryBuilder.FindByIdAsync<ApplicationEntity>(application.Id);
            Assert.Null(actualApplication);
            var actualLog1 = await QueryBuilder.FindByIdAsync<LogEntity>(log1.Id);
            Assert.Null(actualLog1);
        }

        private async Task<LogEntity> CreateLog(ApplicationEntity application, LogLevel level)
        {
            return await LogService.AddAsync(
                application,
                "SomeMessage",
                level,
                DateTime.UtcNow
            );
        }
    }
}
