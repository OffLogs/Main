using NHibernate.Linq;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.Business.Orm.Queries.Entities.Application
{
    public class ApplicationGetListQuery : LinqAsyncQueryBase<ApplicationGetListCriteria, ListDto<ApplicationEntity>>
    {
        public ApplicationGetListQuery(IDbSessionProvider transactionProvider)
            : base(transactionProvider)
        {
        }

        public override async Task<ListDto<ApplicationEntity>> AskAsync(
            ApplicationGetListCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            var pageSize = GlobalConstants.ListPageSize;
            var page = criterion.Page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            var session = TransactionProvider.CurrentSession;
            var query = session.Query<ApplicationEntity>()
                .Where(
                    record => record.User.Id == criterion.UserId 
                        || record.SharedForUsers.Any(
                            sharedUser => sharedUser.Id == criterion.UserId
                        )
                );
            var listQuery = query
                .Skip(offset)
                .Take(pageSize)
                .OrderBy(log => log.CreateTime)
                .ToFuture();
            var count = await query.CountAsync(cancellationToken);
            var list = await listQuery.GetEnumerableAsync(cancellationToken);

            return new ListDto<ApplicationEntity>(list.ToList(), count);
        }
    }
}
