using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core.Service;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Core
{
    [Collection("Default collection")]
    public class MyIntegrationTest:IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _factory;

        protected readonly ICommonDao Dao;
        protected readonly IUserDao UserDao;
        protected readonly ILogDao LogDao;
        protected readonly IJwtAuthService JwtAuthService;
        protected readonly IDataFactoryService DataFactory;
        protected readonly IDataSeederService DataSeeder;
        
        public MyIntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            // jwtService = _factory.Services.GetService(typeof(IJwtService)) as IJwtService;
            Dao = _factory.Services.GetService(typeof(ICommonDao)) as ICommonDao;
            UserDao = _factory.Services.GetService(typeof(IUserDao)) as IUserDao;
            LogDao = _factory.Services.GetService(typeof(ILogDao)) as ILogDao;
            DataFactory = _factory.Services.GetService(typeof(IDataFactoryService)) as IDataFactoryService;
            DataSeeder = _factory.Services.GetService(typeof(IDataSeederService)) as IDataSeederService;
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
    }
}