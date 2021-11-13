using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Notification.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Notifications;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Kafka.Models;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Services.Kafka
{
    public class LogMessageDtoTests
    {
        [Fact]
        public void ShouldCreateDtoAndConvertToEntity()
        {
            var expectedEntity = new LogEntity();
            expectedEntity.Application = new ApplicationEntity()
            {
                Id = 987
            };
            expectedEntity.Level = LogLevel.Debug;
            expectedEntity.EncryptedMessage = new byte[] { 111 };
            expectedEntity.EncryptedSymmetricKey = new byte[] { 0x46 };
            expectedEntity.LogTime = DateTime.Now;
            expectedEntity.Traces = new List<LogTraceEntity>()
            {
                new() { EncryptedTrace = new byte[] { 222 } } 
            };
            expectedEntity.Properties = new List<LogPropertyEntity>()
            {
                new()
                {
                    EncryptedKey = new byte[] { 0x11, 0x11, 0x11 },
                    EncryptedValue = new byte[] { 0x22, 0x22, 0x33 },
                } 
            };
            
            var dto = new LogMessageDto(expectedEntity);
            var serializedJson = JsonConvert.SerializeObject(dto);
            var actualDto = JsonConvert.DeserializeObject<LogMessageDto>(serializedJson);
            Assert.NotNull(actualDto);
            var actualEntity = actualDto.GetEntity();
            
            Assert.Equal(expectedEntity.Application.Id, actualEntity.Application.Id);
            Assert.Equal(expectedEntity.Level, actualEntity.Level);
            Assert.True(
                expectedEntity.EncryptedMessage.CompareTo(actualEntity.EncryptedMessage)
            );
            Assert.True(
                expectedEntity.EncryptedSymmetricKey.CompareTo(actualEntity.EncryptedSymmetricKey)
            );
            Assert.Equal(expectedEntity.LogTime, actualEntity.LogTime);
            Assert.True(
                expectedEntity.Traces.First().EncryptedTrace.CompareTo(
                    actualEntity.Traces.First().EncryptedTrace
                )
            );
            Assert.True(
                expectedEntity.Properties.First().EncryptedKey.CompareTo(
                    actualEntity.Properties.First().EncryptedKey
                )
            );
            Assert.True(
                expectedEntity.Properties.First().EncryptedValue.CompareTo(
                    actualEntity.Properties.First().EncryptedValue
                )
            );
        }
    }
}
