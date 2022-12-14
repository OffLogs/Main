using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Frontend.Core
{
    public class MyIntegrationTest:IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _factory;

        protected readonly IJwtAuthService JwtAuthService;
        protected readonly IKafkaProducerService KafkaProducerService;
        protected readonly IKafkaConsumerService KafkaConsumerService;
        
        public MyIntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            JwtAuthService = _factory.Services.GetService(typeof(IJwtAuthService)) as IJwtAuthService;
            KafkaProducerService = _factory.Services.GetService(typeof(IKafkaProducerService)) as IKafkaProducerService;
            KafkaConsumerService = _factory.Services.GetService(typeof(IKafkaConsumerService)) as IKafkaConsumerService;
        }

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
    }
}