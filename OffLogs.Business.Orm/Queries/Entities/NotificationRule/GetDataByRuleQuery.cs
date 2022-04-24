using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.NotificationRule
{
    public class GetDataByRuleQuery : LinqAsyncQueryBase<GetDataByRuleCriteria, ProcessingDataDto>
    {
        public GetDataByRuleQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<ProcessingDataDto> AskAsync(
            GetDataByRuleCriteria criterion,
            CancellationToken cancellationToken = default
        )
        {
            var session = TransactionProvider.CurrentSession;

            var user = criterion.Rule.User;
            var application = criterion.Rule.Application;
            var conditions = criterion.Rule.Conditions;

            LogEntity logAlias = null;
            ApplicationEntity applicationAlias = null;
            ProcessingDataDto projectionDto = null;
            var query = session.QueryOver<LogEntity>(() => logAlias)
                .SelectList(list =>
                {
                    list.SelectCount(log => log.Id).WithAlias(() => projectionDto.LogCount);
                    return list;
                })
                .JoinAlias(c => c.Application, () => applicationAlias)
                .Where(() => applicationAlias.User.Id == user.Id)
                .And(log => log.CreateTime >= criterion.Rule.LastExecutionTime);
            if (application != null)
            {
                query.And(() => applicationAlias.Id == application.Id);
            }

            Junction conditionExpression = criterion.Rule.LogicOperator == LogicOperatorType.Disjunction
                ? Expression.Disjunction()
                : Expression.Conjunction();
            PrepareExpressionForCondition(conditions, ref conditionExpression);
            query.And(conditionExpression);

            var resultList = await query.TransformUsing(
                Transformers.AliasToBean<ProcessingDataDto>()
            )
            .ListAsync<ProcessingDataDto>(cancellationToken);
             return resultList.First();
        }

        private void PrepareExpressionForCondition(IEnumerable<NotificationConditionEntity> conditions, ref Junction expression)
        {
            foreach (var condition in conditions)
            {
                if (condition.ConditionField == ConditionFieldType.LogLevel)
                {
                    Enum.TryParse(condition.Value, out LogLevel logLevel);
                    if (logLevel == default)
                    {
                        throw new Exception("Incorrect LogLevel value: " + condition.Value);
                    }

                    expression.Add(Expression.Eq("Level", logLevel));
                }
            }
        }
    }
}
