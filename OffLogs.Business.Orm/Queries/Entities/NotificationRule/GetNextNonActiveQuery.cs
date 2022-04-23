using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using NHibernate.Transform;
using OffLogs.Business.Orm.Dto.Entities;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries.Entities.Log;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.NotificationRule
{
    public class GetNextNonActiveQuery : LinqAsyncQueryBase<LogIsExistsByTokenCriteria, NotificationRuleEntity>
    {
        public GetNextNonActiveQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<NotificationRuleEntity> AskAsync(Log.LogIsExistsByTokenCriteria criterion, CancellationToken cancellationToken = default)
        {
            var resultListIds = await TransactionProvider.CurrentSession
                .GetNamedQuery("NotificationRule.getNextNonActive")
                .SetResultTransformer(Transformers.AliasToBean<long?>())
                .ListAsync<long?>(cancellationToken);

            var ruleId = resultListIds.FirstOrDefault();
            if (ruleId.HasValue)
            {
                return await TransactionProvider.CurrentSession.GetAsync<NotificationRuleEntity>(
                    ruleId,
                    cancellationToken
                );
            }
            return null;
        }
    }
}
