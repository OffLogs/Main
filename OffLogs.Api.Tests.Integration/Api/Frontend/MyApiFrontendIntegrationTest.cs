using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Kafka;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;
using Xunit;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OffLogs.Business.Services.Kafka.Consumer;
using OffLogs.Business.Services.Http.ThrottleRequests;

namespace OffLogs.Api.Tests.Integration.Api.Frontend
{
    [Collection("Api.Frontend")]
    public class MyApiFrontendIntegrationTest: IClassFixture<ApiFrontendCustomWebApplicationFactory>, IDisposable
    {
        protected readonly ApiFrontendCustomWebApplicationFactory _factory;
        protected readonly IDataSeederService DataSeeder;
        protected readonly IKafkaProducerService KafkaProducerService;
        protected readonly IKafkaLogsConsumerService KafkaLogsConsumerService;
        protected readonly IDbSessionProvider DbSessionProvider;
        protected readonly ILogService LogService;
        protected readonly IAsyncQueryBuilder QueryBuilder;
        protected readonly IThrottleRequestsService ThrottleRequestsService;
        protected readonly ILogAssembler LogAssembler;

        public MyApiFrontendIntegrationTest(ApiFrontendCustomWebApplicationFactory factory)
        {
            _factory = factory;

            DbSessionProvider = _factory.Services.GetService(typeof(IDbSessionProvider)) as IDbSessionProvider;
            KafkaProducerService = _factory.Services.GetService(typeof(IKafkaProducerService)) as IKafkaProducerService;
            KafkaLogsConsumerService = _factory.Services.GetService(typeof(IKafkaLogsConsumerService)) as IKafkaLogsConsumerService;
            LogService = _factory.Services.GetService(typeof(ILogService)) as ILogService;
            DataSeeder = _factory.Services.GetService(typeof(IDataSeederService)) as IDataSeederService;
            QueryBuilder = _factory.Services.GetService(typeof(IAsyncQueryBuilder)) as IAsyncQueryBuilder;
            ThrottleRequestsService = _factory.Services.GetService(typeof(IThrottleRequestsService)) as IThrottleRequestsService;
            LogAssembler = _factory.Services.GetRequiredService<ILogAssembler>();
        }

        public void Dispose()
        {
            ThrottleRequestsService.Clean();
            DbSessionProvider.PerformCommitAsync().Wait();
            GC.SuppressFinalize(this);
        }
        
        #region Db
        
        protected async Task CommitDbChanges()
        { 
            await DbSessionProvider.PerformCommitAsync();
            DbSessionProvider.CurrentSession.Clear();
        }
        
        #endregion
        
        public async Task<HttpResponseMessage> PostRequestAsAnonymousAsync(string url, object data = null)
        {
            var client = _factory.CreateClient();
            var requestData = JsonContent.Create(data ?? new { });
            return await client.PostAsync(url, requestData);
        }
        
        public async Task<HttpResponseMessage> PostRequestAsync(string url, string jwtToken,  object data = null)
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var requestData = JsonContent.Create(data ?? new {});
            return await client.PostAsync(url, requestData);
        }
        
        public async Task<HttpResponseMessage> GetRequestAsAnonymousAsync(string url)
        {
            var client = _factory.CreateClient();
            return await client.GetAsync(url);
        }
        
        public async Task<HttpResponseMessage> GetRequestAsync(string url, string jwtToken,  object data = null)
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return await client.GetAsync(url);
        }
        
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
