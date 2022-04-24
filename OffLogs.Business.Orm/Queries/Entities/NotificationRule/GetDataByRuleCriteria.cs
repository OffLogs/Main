using System;
using OffLogs.Business.Orm.Entities.Notifications;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.NotificationRule
{
    public class GetDataByRuleCriteria : ICriterion
    {
        public NotificationRuleEntity Rule { get; }

        public GetDataByRuleCriteria(NotificationRuleEntity rule)
        {
            Rule = rule;
        }
    }
}
