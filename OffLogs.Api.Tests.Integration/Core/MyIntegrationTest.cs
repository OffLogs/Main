using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
        protected readonly IJwtAuthService JwtAuthService;
        protected readonly IDataFactoryService DataFactory;
        
        public MyIntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            // jwtService = _factory.Services.GetService(typeof(IJwtService)) as IJwtService;
            Dao = _factory.Services.GetService(typeof(ICommonDao)) as ICommonDao;
            UserDao = _factory.Services.GetService(typeof(IUserDao)) as IUserDao;
            DataFactory = _factory.Services.GetService(typeof(IDataFactoryService)) as IDataFactoryService;
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
        
        // protected (MobileProfileEntity, String) CreateMobileProfileWithToken()
        // {   
        //     var mobileProfile = CommonDataSeeder.CreateMobileProfile();
        //     var token = jwtService.BuildJwt(mobileProfile.Id);
        //     return (mobileProfile, token);
        // }
    }
}