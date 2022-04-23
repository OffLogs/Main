﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
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

    public async Task CreateRule(
        UserEntity user,
        int period,
        LogicOperatorType logicOperator,
        NotificationMessageEntity message,
        ICollection<NotificationConditionEntity> conditions,
        ApplicationEntity application = null
    )
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        
        if (application != null && application.User.Id != user.Id)
        {
            throw new ArgumentException("The user is not application owner");
        }

        if (!conditions.Any())
        {
            throw new ArgumentException("List of conditions can not be empty");
        }

        foreach (var condition in conditions)
        {
            condition.CreateTime = DateTime.UtcNow;
            condition.UpdateTime = DateTime.UtcNow;
        }
        var rule = new NotificationRuleEntity
        {
            User = user,
            Application = application,
            Message = message,
            Period = period,
            LastExecutionTime = DateTime.UtcNow,
            Type = NotificationType.Email,
            IsExecuting = false,
            LogicOperator = logicOperator,
            Conditions = conditions,
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow
        };
        await _commandBuilder.SaveAsync(rule);
    }
    
    public async Task<NotificationRuleEntity> GetNextAndSetExecuting()
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
}
