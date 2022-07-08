using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NHibernate.Util;
using Notification.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Services.Web;
using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Senders.NotificationRule;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Kafka;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Services.Notifications;

public class NotificationRuleProcessingService: INotificationRuleProcessingService
{
    private readonly INotificationRuleService _notificationRuleService;
    private readonly IKafkaProducerService _producerService;
    private readonly IMarkdownService _markdownService;
    private readonly IDbSessionProvider _sessionProvider;
    private readonly string _logsPageUrl;
    private readonly string _frontendUrl;

    public NotificationRuleProcessingService(
        INotificationRuleService notificationRuleService,
        IKafkaProducerService producerService,
        IMarkdownService markdownService,
        IConfiguration configuration,
        IDbSessionProvider sessionProvider
    )
    {
        _notificationRuleService = notificationRuleService;
        _producerService = producerService;
        _markdownService = markdownService;
        _sessionProvider = sessionProvider;

        _logsPageUrl = configuration.GetValue<string>("App:Urls:LogsPage");
        _frontendUrl = configuration.GetValue<string>("App:FrontendUrl");
    }

    public async Task FindAndProcessWaitingRules(CancellationToken cancellationToken = default)
    {
        NotificationRuleEntity rule = null;
        do
        {
            rule = await _notificationRuleService.GetNextAndSetExecutingAsync(cancellationToken);
            if (rule != null)
            {
                var dataByRule = await _notificationRuleService.GetDataForNotificationRule(rule);
                if (dataByRule.LogCount > 0)
                {
                    var notificationContexts = new List<INotificationContext>();
                    if (rule.Type == NotificationType.Email)
                    {
                        notificationContexts = notificationContexts.Concat(
                            GetEmailNotifications(rule, dataByRule)
                        ).ToList();
                    }

                    foreach (var context in notificationContexts)
                    {
                        await _producerService.ProduceNotificationMessageAsync(context);
                    }
                }

                await _notificationRuleService.SetAsExecutedAsync(rule, cancellationToken);
                await _sessionProvider.PerformCommitAsync(cancellationToken);
            }
        } while (rule != null);
    }

    private string PrepareText(
        NotificationRuleEntity rule, 
        ProcessingDataDto ruleData,
        bool isSubject = true
    ) 
    {
        var builder = new TemplatedTextBuilder(
            isSubject ? rule.MessageTemplate.Subject : rule.MessageTemplate.Body    
        );
        builder.AddPlaceholder("ruleName", rule.Title);
        builder.AddPlaceholder("applicationName", rule.Application?.Name);
        builder.AddPlaceholder("userName", rule.User?.UserName);
        builder.AddPlaceholder("dateTime", rule.LastExecutionTime.ToString("MM/dd/yyyy H:mm"));
        
        var navigateUrl = $"{_frontendUrl}/{_logsPageUrl}";
        var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (rule.Application != null)
        {
            queryString.Add("application-id", $"{rule.Application?.Id}");
        }
        builder.AddPlaceholder("navigateUrl", $"{navigateUrl}?{queryString}");
        
        builder.AddPlaceholder("logsCount", ruleData.LogCount.ToString());
        if (isSubject)
        {
            return builder.Build();    
        }
        
        return _markdownService.ToHtml(builder.Build());
    }

    private IEnumerable<INotificationContext> GetEmailNotifications(NotificationRuleEntity rule, ProcessingDataDto data)
    {
        var notificationReceivers = rule.Emails.Select(item => item.Email).ToList();
        notificationReceivers.Add(rule.User.Email);
        
        var notificationContexts = new List<INotificationContext>();
        notificationContexts.Add(
            new EmailNotificationContext()
            {
                Subject = "OffLogs - " + PrepareText(rule, data, true),
                Body = PrepareText(rule, data, false),
                To = notificationReceivers.ToList()
            }    
        );

        return notificationContexts;
    }
}
