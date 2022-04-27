using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities.Notifications;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.NotificationRule
{
    public class GetMessageTemplateListQuery 
        : LinqAsyncQueryBase<FindByIdCriteria, ListDto<MessageTemplateEntity>>
    {
        public GetMessageTemplateListQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<ListDto<MessageTemplateEntity>> AskAsync(
            FindByIdCriteria criterion,
            CancellationToken cancellationToken = default
        )
        {
            var session = TransactionProvider.CurrentSession;

            var query = session.QueryOver<MessageTemplateEntity>()
                .Where((item) => item.User.Id == criterion.Id);

            return new ListDto<MessageTemplateEntity>(
                await query.ListAsync(cancellationToken),
                await query.RowCountAsync(cancellationToken)
            );
        }
    }
}
