﻿using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Constants;
using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.RequestLogDaoTest
{
    public class GetByTokenTests: MyApiIntegrationTest
    {
        public GetByTokenTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public async Task ShouldGetByToken()
        {
            var data = "{some data}";
            var clientIp = "127.0.0.1";
            var token = SecurityUtil.GetTimeBasedToken();
            
            await RequestLogDao.AddAsync(RequestLogType.Log, clientIp, "{some data}", token);
            
            var existsLog = await RequestLogDao.GetByTokenAsync(token);
            Assert.True(existsLog.Id > 0);
            Assert.NotNull(existsLog.CreateTime);
            Assert.Equal(RequestLogType.Log, existsLog.Type);
            Assert.Equal(data, existsLog.Data);
            Assert.Equal(clientIp, existsLog.ClientIp);
        }
    }
}