using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.Business.Services.Redis.Clients;
using OffLogs.WorkerService.Core;

namespace OffLogs.WorkerService.Services.Redis
{
    internal class UserInfoSeedingHostedService : ABackgroundService
    {
        private readonly IUserInfoRedisClient _userInfoRedisClient;

        public UserInfoSeedingHostedService(
            ILogger<ABackgroundService> logger,
            IUserInfoRedisClient userInfoRedisClient
        ) : base(logger)
        {
            _userInfoRedisClient = userInfoRedisClient;
            ServiceName = "LogsProcessingHostedService";
        }

        protected override string GetCrontabExpression()
        {
            // At 00:00.
            return "*/5 * * * *";
        }
        
        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            LogDebug($"Seeding of the Users info started: {DateTime.Now}");
            await _userInfoRedisClient.SeedUsersPackages(cancellationToken);
        }
    }
}
