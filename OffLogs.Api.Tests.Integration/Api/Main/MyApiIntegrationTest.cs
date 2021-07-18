using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Orm.Criteria.Entites;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main
{
    [Collection("Default collection")]
    public class MyApiIntegrationTest: IClassFixture<ApiCustomWebApplicationFactory>
    {
        protected readonly ApiCustomWebApplicationFactory _factory;
        
        protected readonly IJwtAuthService JwtAuthService;
        protected readonly IDataFactoryService DataFactory;
        protected readonly IDataSeederService DataSeeder;
        protected readonly IKafkaProducerService KafkaProducerService;
        protected readonly IKafkaConsumerService KafkaConsumerService;
        protected readonly IDbSessionProvider DbSessionProvider;
        protected readonly IAsyncQueryBuilder QueryBuilder;
        protected readonly IAsyncCommandBuilder CommandBuilder;
        
        protected readonly ILogService LogService;
        
        public MyApiIntegrationTest(ApiCustomWebApplicationFactory factory)
        {
            _factory = factory;
            DbSessionProvider = _factory.Services.GetService(typeof(IDbSessionProvider)) as IDbSessionProvider;
            QueryBuilder = _factory.Services.GetService(typeof(IAsyncQueryBuilder)) as IAsyncQueryBuilder;
            CommandBuilder = _factory.Services.GetService(typeof(IAsyncCommandBuilder)) as IAsyncCommandBuilder;
            DataFactory = _factory.Services.GetService(typeof(IDataFactoryService)) as IDataFactoryService;
            DataSeeder = _factory.Services.GetService(typeof(IDataSeederService)) as IDataSeederService;
            JwtAuthService = _factory.Services.GetService(typeof(IJwtAuthService)) as IJwtAuthService;
            KafkaProducerService = _factory.Services.GetService(typeof(IKafkaProducerService)) as IKafkaProducerService;
            KafkaConsumerService = _factory.Services.GetService(typeof(IKafkaConsumerService)) as IKafkaConsumerService;
            LogService = _factory.Services.GetService(typeof(ILogService)) as ILogService;
        }

        public void Dispose()
        {
            DbSessionProvider.PerformCommitAsync().Wait();
            GC.SuppressFinalize(this);
        }
        
        #region Http
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