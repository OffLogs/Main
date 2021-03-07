﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OffLogs.Api.Models.Response;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Controller.LogController
{
    public class SerilogActionTests: MyIntegrationTest
    {
        public SerilogActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("/log/serilog")]
        public async Task ShouldAddWarningLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

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
            await response.GetJsonDataAsync<object>();
            response.EnsureSuccessStatusCode();
        }
        
        [Theory]
        [InlineData("/log/serilog")]
        public async Task ShouldAddFatalLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

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
        }
        
        [Theory]
        [InlineData("/log/serilog")]
        public async Task ShouldAddInformationLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

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
        }
        
        [Theory]
        [InlineData("/log/serilog")]
        public async Task ShouldAddErrorLog(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

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
        }
    }
}
