using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserSearchQuery : LinqAsyncQueryBase<UserSearchCriteria, ICollection<UserEntity>>
    {
        public UserSearchQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<ICollection<UserEntity>> AskAsync(UserSearchCriteria criterion, CancellationToken cancellationToken = default)
        {
            UserEntity userAlias = null;
            var query = TransactionProvider.CurrentSession
                .QueryOver<UserEntity>(() => userAlias)
                .Where(
                    Restrictions.Or(
                        Restrictions.Like("UserName", criterion.Search),
                        Restrictions.Like("Email", criterion.Search)
                    )
                );

            if (criterion.ExceludeIds != null && criterion.ExceludeIds.Count() > 0)
            {
                query = query.And(
                    Restrictions.Not(
                        Restrictions.In("Id", criterion.ExceludeIds)
                    )
                );
            }
            return await query
                .Take(100)
                .ListAsync();
        }
    }
}