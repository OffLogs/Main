using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using Serilog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Frontend.Controller.LogController
{
    public class SerilogActionTests: MyApiFrontendIntegrationTest
    {
        public SerilogActionTests(ApiFrontendCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData("/log/add/serilog")]
        public async Task ShouldAddWarningLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                events = new List<object>()
                {
                    new
                    {
                        Timestamp = "2021-03-01T21:50:42.1422383+02:00",
                        Level = "Warning",
                        MessageTemplate = "This is Warning message",
                        RenderedMessage = "This is Warning message",
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
            await KafkaConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-1));
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Theory]
        [InlineData("/log/add/serilog")]
        public async Task ShouldAddFatalLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                events = new List<object>()
                {
                    new {
                        Timestamp = "2021-03-01T21:50:42.1437253+02:00",
                        Level = "Fatal",
                        MessageTemplate = "This is Critical message",
                        RenderedMessage = "This is Critical message",
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
            await KafkaConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-1));
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Theory]
        [InlineData("/log/add/serilog")]
        public async Task ShouldAddInformationLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                events = new List<object>()
                {
                    new {
                        Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                        Level = "Information",
                        MessageTemplate = "This is Information message",
                        RenderedMessage = "This is Information message",
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
                        Level = "Information",
                        MessageTemplate = "This is Information message",
                        RenderedMessage = "This is Information message",
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
            await KafkaConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(2, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-1));
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Theory]
        [InlineData("/log/add/serilog")]
        public async Task ShouldAddErrorLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                events = new List<object>()
                {
                    new  {
                        Timestamp = "2021-03-01T21:50:42.1443263+02:00",
                        Level = "Error",
                        MessageTemplate = "The method or operation is not implemented.",
                        RenderedMessage = "The method or operation is not implemented.",
                        Exception = "System.NotImplementedException: The method or operation is not implemented.\n at OffLogs.Api.Controller.HomeController.Func41() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 58\n   at OffLogs.Api.Controller.HomeController.Func4() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 53\n   at OffLogs.Api.Controller.HomeController.Func3() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 48\n at OffLogs.Api.Controller.HomeController.Func2() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 43\n   at OffLogs.Api.Controller.HomeController.Func1() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 38\n at OffLogs.Api.Controller.HomeController.Ping() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 22",
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
            await KafkaConsumerService.ProcessLogsAsync(false);
            
            var actualList  = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-1));
            Assert.True(actualLog.Properties.Count == 9);
            Assert.True(actualLog.Traces.Count == 7);
        }
        
        [Theory]
        [InlineData("/log/add/serilog")]
        public async Task ShouldNotContainLotOfItems(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            // Act
            var events = new List<object>();
            for (int i = 0; i < 102; i++)
            {
                events.Add(new
                {
                    Timestamp = "2021-03-01T21:50:42.1422383+02:00",
                    Level = "Warning",
                    MessageTemplate = "This is Warning message",
                    RenderedMessage = "This is Warning message",
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
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new { events });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Theory]
        [InlineData("/log/add/serilog")]
        public async Task ShouldAddLogsIfPropertiesIsObjects(string url)
        {
            // Arrange
            var property1 = @"{""Id"": 50, ""Name"": ""UsingInMemoryRepository""}";
            var property2 = new { Id = 50, Name = "UsingInMemoryRepository" };
            var user = await DataSeeder.CreateNewUser();

            var list = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(0, list.TotalCount);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                events = new List<object>()
                {
                    new  {
                        Timestamp = "2021-03-01T21:50:42.1443263+02:00",
                        Level = "Error",
                        MessageTemplate = "The method or operation is not implemented.",
                        RenderedMessage = "The method or operation is not implemented.",
                        Exception = "System.NotImplementedException: The method or operation is not implemented.\n at OffLogs.Api.Controller.HomeController.Func41() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 58\n   at OffLogs.Api.Controller.HomeController.Func4() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 53\n   at OffLogs.Api.Controller.HomeController.Func3() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 48\n at OffLogs.Api.Controller.HomeController.Func2() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 43\n   at OffLogs.Api.Controller.HomeController.Func1() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 38\n at OffLogs.Api.Controller.HomeController.Ping() in /home/lampego/work/net/OffLogs/OffLogs.Api/Controller/HomeController.cs:line 22",
                        Properties = new {
                            SourceContext = property1,
                            EventId = property2
                        }
                    },
                }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            
            // Process messages from Kafka
            KafkaProducerService.Flush();
            await KafkaConsumerService.ProcessLogsAsync(false);
            
            var actualList = await GetLogsList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualList.TotalCount);
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(actualList.Items.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.CreateTime > System.DateTime.UtcNow.AddMinutes(-1));
            Assert.True(actualLog.Properties.Count == 2);
            foreach (var property in actualLog.Properties)
            {
                var isFirstTrue = "\"{\\\"Id\\\": 50, \\\"Name\\\": \\\"UsingInMemoryRepository\\\"}\"" == property.Value;
                var isSecondTrue = @"{""id"":50,""name"":""UsingInMemoryRepository""}" == property.Value;
                Assert.True(isFirstTrue || isSecondTrue);
            }
        }
    }
}
