using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Transform;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities.Notifications;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.NotificationRule
{
    public class GetNextNonActiveQuery : LinqAsyncQueryBase<GetNextNonActiveCriteria, NotificationRuleEntity>
    {
        public GetNextNonActiveQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<NotificationRuleEntity> AskAsync(GetNextNonActiveCriteria criterion, CancellationToken cancellationToken = default)
        {
            var session = TransactionProvider.CurrentSession;
            
            var resultListIds = await session
                .GetNamedQuery("NotificationRule.getNextNonActive")
                .SetParameter("dateNow", DateTime.UtcNow)
                .SetResultTransformer(Transformers.AliasToBean<IdDto>())
                .ListAsync<IdDto>(cancellationToken);

            var ruleIdModel = resultListIds.FirstOrDefault();
            if (ruleIdModel != null)
            {
                var rule = await session.GetAsync<NotificationRuleEntity>(ruleIdModel.Id, cancellationToken);
                return rule;
            }
            return null;
        }
    }
}
