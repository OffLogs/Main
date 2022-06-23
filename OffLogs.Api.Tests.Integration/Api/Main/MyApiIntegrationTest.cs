using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Commands.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Notification.Abstractions;
using OffLogs.Api.Tests.Integration.Core.Faker;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Notifications.Services;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Business.Services.Entities.UserEmail;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.Business.Services.Security;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main
{
    [Collection("Default collection")]
    public class MyApiIntegrationTest: IClassFixture<ApiCustomWebApplicationFactory>, IDisposable
    {
        protected readonly ApiCustomWebApplicationFactory _factory;
        
        protected readonly IJwtAuthService JwtAuthService;
        protected readonly IDataFactoryService DataFactory;
        protected readonly IDataSeederService DataSeeder;
        protected readonly IKafkaProducerService KafkaProducerService;
        protected readonly IKafkaLogsConsumerService KafkaLogsConsumerService;
        protected readonly IKafkaNotificationsConsumerService KafkaNotificationsConsumerService;
        protected readonly IDbSessionProvider DbSessionProvider;
        protected readonly IAsyncQueryBuilder QueryBuilder;
        protected readonly IAsyncCommandBuilder CommandBuilder;
        protected readonly IAsyncNotificationBuilder NotificationBuilder;
        protected readonly IThrottleRequestsService ThrottleRequestsService;

        protected readonly ILogService LogService;
        protected readonly ILogShareService LogShareService;
        protected readonly IUserService UserService;
        protected readonly IUserEmailService UserEmailService;
        protected readonly IApplicationService ApplicationService;

        protected readonly IAccessPolicyService AccessPolicyService;

        protected readonly FakeEmailSendingService EmailSendingService;
        
        protected readonly ILogAssembler LogAssembler;

        public MyApiIntegrationTest(ApiCustomWebApplicationFactory factory)
        {
            _factory = factory;
            DbSessionProvider = _factory.Services.GetRequiredService<IDbSessionProvider>();
            QueryBuilder = _factory.Services.GetRequiredService<IAsyncQueryBuilder>();
            CommandBuilder = _factory.Services.GetRequiredService<IAsyncCommandBuilder>();
            DataFactory = _factory.Services.GetRequiredService<IDataFactoryService>();
            DataSeeder = _factory.Services.GetRequiredService<IDataSeederService>();
            JwtAuthService = _factory.Services.GetRequiredService<IJwtAuthService>();
            NotificationBuilder = _factory.Services.GetRequiredService<IAsyncNotificationBuilder>();
            KafkaProducerService = _factory.Services.GetRequiredService<IKafkaProducerService>();
            KafkaLogsConsumerService = _factory.Services.GetRequiredService<IKafkaLogsConsumerService>();
            KafkaNotificationsConsumerService = _factory.Services.GetRequiredService<IKafkaNotificationsConsumerService>();
            LogService = _factory.Services.GetRequiredService<ILogService>();
            UserService = _factory.Services.GetRequiredService<IUserService>();
            ApplicationService = _factory.Services.GetRequiredService<IApplicationService>();
            AccessPolicyService = _factory.Services.GetRequiredService<IAccessPolicyService>();
            LogShareService = _factory.Services.GetRequiredService<ILogShareService>();
            EmailSendingService = _factory.Services.GetRequiredService<IEmailSendingService>() as FakeEmailSendingService;
            ThrottleRequestsService = _factory.Services.GetRequiredService<IThrottleRequestsService>();
            LogAssembler = _factory.Services.GetRequiredService<ILogAssembler>();
            UserEmailService = _factory.Services.GetRequiredService<IUserEmailService>();

            ThrottleRequestsService.Clean();
            EmailSendingService?.Reset();
        }

        public void Dispose()
        {
            DbSessionProvider.PerformCommitAsync().Wait();
            GC.SuppressFinalize(this);
        }

        protected async Task CommitDbChanges()
        { 
            await DbSessionProvider.PerformCommitAsync();
            DbSessionProvider.CurrentSession.Clear();
        }

        #region Http
        public async Task<HttpResponseMessage> PostRequestAsAnonymousAsync(string url, object data = null)
        {
            await CommitDbChanges();

            var client = _factory.CreateClient();
            var requestData = JsonContent.Create(data ?? new { });
            return await client.PostAsync(url, requestData);
        }
        
        public async Task<HttpResponseMessage> PostRequestAsync(string url, string jwtToken,  object data = null)
        {
            await CommitDbChanges();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var requestData = JsonContent.Create(data ?? new {});
            return await client.PostAsync(url, requestData);
        }
        
        public async Task<HttpResponseMessage> GetRequestAsAnonymousAsync(string url)
        {
            await CommitDbChanges();

            var client = _factory.CreateClient();
            return await client.GetAsync(url);
        }
        
        public async Task<HttpResponseMessage> GetRequestAsync(string url, string jwtToken,  object data = null)
        {
            await CommitDbChanges();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/json");
            return await client.GetAsync(url);
        }
        #endregion
        
        #region Log
        protected async Task<ListDto<LogEntity>> GetLogsList(long applicationId, int page = 1)
        {
            return await QueryBuilder.For<ListDto<LogEntity>>().WithAsync(new LogGetListCriteria()
            {
                ApplicationId = applicationId,
                Page = page
            });
        }
        #endregion
    }
}
