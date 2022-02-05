using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Http.ThrottleRequests;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Frontend.Controller.LogController
{
    public class CommonActionTests: MyApiFrontendIntegrationTest
    {
        private const string Url = "/log/add";
        
        public CommonActionTests(ApiFrontendCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldAddWarningLog()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            var list = await GetLogsList(user.Application.Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(Url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new
                    {
                        Timestamp = "2021-03-01T21:50:42.1422383+02:00",
                        Level = SiteLogLevel.Warning.GetValue(),
                        Message = "This is Warning message",
                        Properties = new {
                            SourceContext = "OffLogs.Api.Controller.HomeController",
                            ActionId = "a8564f16-ca80-41c6-9f92-393fd3051dd2",
                            ActionName = "OffLogs.Api.Controller.HomeController.Ping (OffLogs.Api)",
                            RequestId = "0HM6STD0804I2:00000003",
                            RequestPath = "/ping",
                            ConnectionId = "0HM6STD0804I2",
                            MachineName = "lampego-mint",
                            ProcessId = 18820,
                            ThreadId = 13
                        }
                    },
                }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            KafkaProducerService.Flush();
            // Process messages from Kafka
            var processedCounter = await KafkaLogsConsumerService.ProcessLogsAsync(false);
            Assert.True(processedCounter > 0);
            
            var actualList = await GetLogsList(user.Application.Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.EncryptedMessage);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-10));
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Fact]
        public async Task ShouldAddFatalLog()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(Url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new {
                        Timestamp = "2021-03-01T21:50:42.1437253+02:00",
                        Level = SiteLogLevel.Fatal.GetValue(),
                        Message = "This is Critical message",
                        Properties = new {
                            SourceContext = "OffLogs.Api.Controller.HomeController",
                            ActionId = "a8564f16-ca80-41c6-9f92-393fd3051dd2",
                            ActionName = "OffLogs.Api.Controller.HomeController.Ping (OffLogs.Api)",
                            RequestId = "0HM6STD0804I2:00000003",
                            RequestPath = "/ping",
                            ConnectionId = "0HM6STD0804I2",
                            MachineName = "lampego-mint",
                            ProcessId = 18820,
                            ThreadId = 13
                        }
                    },
                }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            // Process messages from Kafka
            KafkaProducerService.Flush();
            await KafkaLogsConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.EncryptedMessage);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-10));
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Fact]
        public async Task ShouldAddInformationLog()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(Url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new {
                        Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                        Level = SiteLogLevel.Information.GetValue(),
                        Message = "This is Information message",
                        Properties = new {
                            SourceContext = "OffLogs.Api.Controller.HomeController",
                            ActionId = "a8564f16-ca80-41c6-9f92-393fd3051dd2",
                            ActionName = "OffLogs.Api.Controller.HomeController.Ping (OffLogs.Api)",
                            RequestId = "0HM6STD0804I2:00000003",
                            RequestPath = "/ping",
                            ConnectionId = "0HM6STD0804I2",
                            MachineName = "lampego-mint",
                            ProcessId = 18820,
                            ThreadId = 13
                        }
                    },
                    new {
                        Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                        Level = SiteLogLevel.Information.GetValue(),
                        Message = "This is Information message",
                        Properties = new {
                            SourceContext = "OffLogs.Api.Controller.HomeController",
                            ActionId = "a8564f16-ca80-41c6-9f92-393fd3051dd2",
                            ActionName = "OffLogs.Api.Controller.HomeController.Ping (OffLogs.Api)",
                            RequestId = "0HM6STD0804I2:00000003",
                            RequestPath = "/ping",
                            ConnectionId = "0HM6STD0804I2",
                            MachineName = "lampego-mint",
                            ProcessId = 18820,
                            ThreadId = 13
                        }
                    },
                }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            // Process messages from Kafka
            KafkaProducerService.Flush();
            await KafkaLogsConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(2, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.EncryptedMessage);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-10));
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Fact]
        public async Task ShouldAddErrorLog()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(Url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new  {
                        Timestamp = "2021-03-01T21:50:42.1443263+02:00",
                        Level = SiteLogLevel.Error.GetValue(),
                        Message = "The method or operation is not implemented.",
                        Traces = new List<string>()
                        {
                            "System.NotImplementedException: The method or operation is not implemented.", 
                            " at OffLogs.Api.Controller.HomeController.Func41() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 58", 
                            "   at OffLogs.Api.Controller.HomeController.Func4() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 53", 
                            "   at OffLogs.Api.Controller.HomeController.Func3() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 48", 
                            " at OffLogs.Api.Controller.HomeController.Func2() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 43", 
                            "   at OffLogs.Api.Controller.HomeController.Func1() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 38", 
                            " at OffLogs.Api.Controller.HomeController.Ping() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 22"
                        },
                        Properties = new {
                            SourceContext = "OffLogs.Api.Controller.HomeController",
                            ActionId = "a8564f16-ca80-41c6-9f92-393fd3051dd2",
                            ActionName = "OffLogs.Api.Controller.HomeController.Ping (OffLogs.Api)",
                            RequestId = "0HM6STD0804I2:00000003",
                            RequestPath = "/ping",
                            ConnectionId = "0HM6STD0804I2",
                            MachineName = "lampego-mint",
                            ProcessId = 18820,
                            ThreadId = 13
                        }
                    },
                }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            // Process messages from Kafka
            KafkaProducerService.Flush();
            await DbSessionProvider.PerformCommitAsync();
            await KafkaLogsConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.EncryptedMessage);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-10));
            Assert.True(actualLog.Properties.Count == 9);
            Assert.True(actualLog.Traces.Count == 7);
        }
        
        [Fact]
        public async Task ShouldNotContainLotOfItems()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            // Act
            var logs = new List<object>();
            for (int i = 0; i < 104; i++)
            {
                logs.Add(new {
                    Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                    Level = SiteLogLevel.Information.GetValue(),
                    Message = "This is Information message",
                    Properties = new {
                        SourceContext = "OffLogs.Api.Controller.HomeController",
                        ActionId = "a8564f16-ca80-41c6-9f92-393fd3051dd2",
                        ActionName = "OffLogs.Api.Controller.HomeController.Ping (OffLogs.Api)",
                        RequestId = "0HM6STD0804I2:00000003",
                        RequestPath = "/ping",
                        ConnectionId = "0HM6STD0804I2",
                        MachineName = "lampego-mint",
                        ProcessId = 18820,
                        ThreadId = 13
                    }
                });
            }
            var response = await PostRequestAsync(Url, user.ApplicationApiToken, new { logs });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowExceptionIfTooManyRequests()
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();

            // Act
            var logs = new List<object>()
            {
                new
                {
                    Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                    Level = SiteLogLevel.Information.GetValue(),
                    Message = "This is Information message"
                }
            };

            HttpResponseMessage response;
            response = await PostRequestAsync(Url, user.ApplicationApiToken, new { logs });
            response.EnsureSuccessStatusCode();

            for (int i = 0; i < 500; i++)
            {
                await ThrottleRequestsService.CheckOrThrowExceptionAsync(RequestItemType.Application, user.ApplicationId);
            }

            response = await PostRequestAsync(Url, user.ApplicationApiToken, new { logs });
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
