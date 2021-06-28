using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Frontend.Core;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Dao;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Frontend.Controller.LogController
{
    public class CommonActionTests: MyIntegrationTest
    {
        public CommonActionTests(CustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData("/log/add")]
        public async Task ShouldAddWarningLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var (_, logsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(0, logsCounter);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new
                    {
                        Timestamp = "2021-03-01T21:50:42.1422383+02:00",
                        Level = LogLevel.Warning.GetValue(),
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
            var (actualLogs, actualLogsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualLogsCounter);
            var actualLog = await LogDao.GetLogAsync(actualLogs.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Theory]
        [InlineData("/log/add")]
        public async Task ShouldAddFatalLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var (_, logsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(0, logsCounter);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new {
                        Timestamp = "2021-03-01T21:50:42.1437253+02:00",
                        Level = LogLevel.Fatal.GetValue(),
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
            var (actualLogs, actualLogsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualLogsCounter);
            var actualLog = await LogDao.GetLogAsync(actualLogs.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Theory]
        [InlineData("/log/add")]
        public async Task ShouldAddInformationLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var (_, logsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(0, logsCounter);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new {
                        Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                        Level = LogLevel.Information.GetValue(),
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
                        Level = LogLevel.Information.GetValue(),
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
            var (actualLogs, actualLogsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(2, actualLogsCounter);
            var actualLog = await LogDao.GetLogAsync(actualLogs.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.Properties.Count  > 0);
            Assert.True(actualLog.Traces.Count == 0);
        }
        
        [Theory]
        [InlineData("/log/add")]
        public async Task ShouldAddErrorLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            var (_, logsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(0, logsCounter);
            // Act
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new
            {
                logs = new List<object>()
                {
                    new  {
                        Timestamp = "2021-03-01T21:50:42.1443263+02:00",
                        Level = LogLevel.Error.GetValue(),
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
            var (actualLogs, actualLogsCounter) = await LogDao.GetList(user.Applications.First().Id, 1);
            Assert.Equal(1, actualLogsCounter);
            var actualLog = await LogDao.GetLogAsync(actualLogs.First().Id);
            Assert.NotEmpty(actualLog.Message);
            Assert.NotNull(actualLog.Level);
            Assert.True(actualLog.Properties.Count == 9);
            Assert.True(actualLog.Traces.Count == 7);
        }
        
        [Theory]
        [InlineData("/log/add")]
        public async Task ShouldNotContainLotOfItems(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            // Act
            var logs = new List<object>();
            for (int i = 0; i < 104; i++)
            {
                logs.Add(new {
                    Timestamp = "2021-03-01T21:50:42.1440609+02:00",
                    Level = LogLevel.Information.GetValue(),
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
            var response = await PostRequestAsync(url, user.ApplicationApiToken, new { logs });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
