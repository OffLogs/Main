using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Notification.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Services.Web;
using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Senders.NotificationRule;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Kafka;

namespace OffLogs.Business.Services.Notifications;

public class NotificationRuleProcessingService: INotificationRuleProcessingService
{
    private readonly INotificationRuleService _notificationRuleService;
    private readonly IKafkaProducerService _producerService;
    private readonly IMarkdownService _markdownService;
    private readonly string _logsPageUrl;
    private readonly string _frontendUrl;

    public NotificationRuleProcessingService(
        INotificationRuleService notificationRuleService,
        IKafkaProducerService producerService,
        IMarkdownService markdownService,
        IConfiguration configuration
    )
    {
        _notificationRuleService = notificationRuleService;
        _producerService = producerService;
        _markdownService = markdownService;

        _logsPageUrl = configuration.GetValue<string>("App:Urls:LogsPage");
        _frontendUrl = configuration.GetValue<string>("App:FrontendUrl");
    }

    public async Task FindAndProcessWaitingRules()
    {
        NotificationRuleEntity rule = null;
        do
        {
            rule = await _notificationRuleService.GetNextAndSetExecutingAsync();
            if (rule != null)
            {
                var dataByRule = await _notificationRuleService.GetDataForNotificationRule(rule);
                if (dataByRule.LogCount > 0)
                {
                    var notificationReceivers = new List<string>()
                    {
                        rule.User.Email
                    };
                    
                    INotificationContext notificationContext = null;
                    if (rule.Type == NotificationType.Email)
                    {
                        notificationContext = new EmailNotificationContext()
                        {
                            Subject = rule.MessageTemplate.Subject,
                            Body = PrepareBody(rule, dataByRule),
                            To = notificationReceivers
                        };
                    }

                    if (notificationContext != null)
                    {
                        await _producerService.ProduceNotificationMessageAsync(notificationContext);
                    }
                }
            }
        } while (rule != null);
    }

    private string PrepareBody(NotificationRuleEntity rule, ProcessingDataDto ruleData) 
    {
        var builder = new TemplatedTextBuilder(rule.MessageTemplate.Body);
        builder.AddPlaceholder("applicationName", rule.Application.Name);
        builder.AddPlaceholder("navigateUrl", $"{_frontendUrl}/{_logsPageUrl}/{rule.Application.Id}");
        builder.AddPlaceholder("logCount", ruleData.LogCount.ToString());
        return _markdownService.ToHtml(builder.Build());
    }
}
