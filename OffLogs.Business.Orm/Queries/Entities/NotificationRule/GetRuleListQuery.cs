using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities.Notifications;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.NotificationRule
{
    public class GetRuleListQuery 
        : LinqAsyncQueryBase<FindByIdCriteria, ListDto<NotificationRuleEntity>>
    {
        public GetRuleListQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<ListDto<NotificationRuleEntity>> AskAsync(
            FindByIdCriteria criterion,
            CancellationToken cancellationToken = default
        )
        {
            var session = TransactionProvider.CurrentSession;

            var query = session.QueryOver<NotificationRuleEntity>()
                .Where(item => item.User.Id == criterion.Id);

            return new ListDto<NotificationRuleEntity>(
                await query.ListAsync(cancellationToken),
                await query.RowCountAsync(cancellationToken)
            );
        }
    }
}
