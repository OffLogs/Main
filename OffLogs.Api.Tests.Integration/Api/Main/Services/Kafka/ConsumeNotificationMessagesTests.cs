using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Notifications.Senders.NotificationRule;
using OffLogs.Business.Notifications.Senders.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ConsumeNotificationMessagesTests : MyApiIntegrationTest
    {
        public ConsumeNotificationMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendRegularLogsNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var dto = new RegularLogsNotificationContext()
            { 
                ToAddress = toAddress,
                ErrorCounter = 3
            };

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);

            Assert.True(EmailSendingService.IsEmailSent);
            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.Subject.Contains("Recent logs report")
                    && sentMessage.Body.Contains("logs were received recently")
                    && toAddress.Contains(sentMessage.To)
            );
        }

        [Fact]
        public async Task ShouldSendRegularLogsDeletedNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var sentDate = DateTime.UtcNow;
            var dto = new LogsDeletedNotificationContext(toAddress, sentDate);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.Subject.Contains("Logs deletion notification")
                    && sentMessage.Body.Contains(sentDate.ToString("G"))
                    && toAddress.Contains(sentMessage.To)
            );
        }

        [Fact]
        public async Task ShouldSendRegularApplicationDeletedNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var applicationName = "TestAppName";
            var dto = new ApplicationDeletedNotificationContext(toAddress, applicationName);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.Subject.Contains("Application has been deleted")
                    && sentMessage.Body.Contains(applicationName)
                    && toAddress.Contains(sentMessage.To)
            );
        }
        
        [Fact]
        public async Task ShouldSendRegistrationNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var frontUrl = "https://font.url";
            var token = "someToken";
            var notificationContext = new RegistrationNotificationContext(toAddress, frontUrl, token);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.Subject.Contains("OffLogs verification")
                    && sentMessage.Body.Contains(notificationContext.VerificationUrl)
                    && sentMessage.To.Contains(toAddress)
            );
        }
        
        [Fact]
        public async Task ShouldSendTestNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var frontUrl = "https://font.url";
            var token = "someToken";
            var notificationContext = new TestNotificationContext(toAddress);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);
            
            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.Subject.Contains("Test notification")
                    && sentMessage.To.Contains(toAddress)
            );
        }
        
        [Fact]
        public async Task ShouldSendTestNotificationForNotificationRuleAndReceiveIt()
        {
            var expectBody = "test body";
            var expectSubject = "test subject";
            var expectTo1 = "test1@test.com";
            var expectTo2 = "test2@test.com";
            var notificationContext = new EmailNotificationContext()
            {
                Body = expectBody,
                Subject = expectSubject,
                To = new List<string>()
                {
                    expectTo1,
                    expectTo2
                }
            };

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);
            
            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.Subject.Contains(expectSubject)
                    && sentMessage.Body.Contains(expectBody)
                    && (sentMessage.To == expectTo1 || sentMessage.To == expectTo2)
            );
        }
        
        [Fact]
        public async Task ShouldSendTestNotificationForEmailVerificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var frontUrl = "https://font.url";
            var token = "someToken";
            var notificationContext = new EmailVerificationNotificationContext(toAddress, frontUrl, token);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            Assert.Contains(
                EmailSendingService.SentMessages, 
                sentMessage => sentMessage.Subject.Contains("verification") 
                    && sentMessage.To == toAddress
            );
        }
        
        [Fact]
        public async Task ShouldSendTestNotificationForEmailVerifiedAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var verifiedAddress = "test333@test.com";
            var notificationContext = new EmailVerifiedNotificationContext(toAddress, verifiedAddress);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            Assert.Contains(
                EmailSendingService.SentMessages, 
                sentMessage => sentMessage.Subject.Contains("verification") 
                    && sentMessage.To == toAddress
            );
        }
    }
}
