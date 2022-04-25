using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.NotificationRule;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.NotificationRule;

public class NotificationRuleService: INotificationRuleService
{
    private readonly IAsyncCommandBuilder _commandBuilder;
    private readonly IAsyncQueryBuilder _queryBuilder;
    private readonly IDbSessionProvider _dbSessionProvider;

    public NotificationRuleService(
        IAsyncCommandBuilder commandBuilder,
        IAsyncQueryBuilder queryBuilder,
        IDbSessionProvider dbSessionProvider
    )
    {
        _commandBuilder = commandBuilder;
        _queryBuilder = queryBuilder;
        _dbSessionProvider = dbSessionProvider;
    }

    public async Task<NotificationRuleEntity> SetRule(
        UserEntity user,
        int period,
        LogicOperatorType logicOperator,
        NotificationMessageEntity message,
        ICollection<NotificationConditionEntity> conditions,
        ApplicationEntity application = null,
        long? existsRuleId = null
    )
    {
        if (message == null || !message.IsOwner(user.Id)) throw new ArgumentNullException(nameof(message));
        
        if (application != null && application.User.Id != user.Id)
        {
            throw new ArgumentException("The user is not application owner");
        }

        if (!conditions.Any())
        {
            throw new ArgumentException("List of conditions can not be empty");
        }

        NotificationRuleEntity existsRule = null;
        if (existsRuleId.HasValue)
        {
            existsRule = await _queryBuilder.FindByIdAsync<NotificationRuleEntity>(existsRuleId.Value);
        }

        var rule = new NotificationRuleEntity();
        if (existsRule != null)
        {
            rule = existsRule;
        }
        else
        {
            rule.User = user;
            rule.CreateTime = DateTime.UtcNow;
            rule.LastExecutionTime = DateTime.UtcNow;
            rule.IsExecuting = false;
        }
        
        rule.Application = application;
        rule.Message = message;
        rule.Period = period;
        rule.Type = NotificationType.Email;
        rule.LogicOperator = logicOperator;
        rule.UpdateTime = DateTime.UtcNow;

        foreach (var condition in conditions)
        {
            if (existsRule == null)
            {
                condition.CreateTime = DateTime.UtcNow;
            }
            condition.UpdateTime = DateTime.UtcNow;
            condition.Rule = rule;
            rule.Conditions.Add(condition);
        }
        await _commandBuilder.SaveAsync(rule);
        return rule;
    }

    public async Task<NotificationRuleEntity> GetNextAndSetExecutingAsync()
    {
        var notificationRule = await _queryBuilder.For<NotificationRuleEntity>().WithAsync(new GetNextNonActiveCriteria());
        if (notificationRule == null)
        {
            return null;
        }
        notificationRule.IsExecuting = true;
        await _commandBuilder.SaveAsync(notificationRule);
        await _dbSessionProvider.PerformCommitAsync();
        return notificationRule;
    }

    public async Task<ProcessingDataDto> GetDataForNotificationRule(NotificationRuleEntity rule)
    {
        return await _queryBuilder.For<ProcessingDataDto>()
            .WithAsync(new GetDataByRuleCriteria(rule));
    }
}
