using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserGetPendingQuery : LinqAsyncQueryBase<UserEntity, UserGetPendingCriteria, UserEntity>
    {
        public UserGetPendingQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<UserEntity> AskAsync(
            UserGetPendingCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            return await TransactionProvider.CurrentSession.Query<UserEntity>()
                .Where(
                    q => q.VerificationToken == criterion.Token
                           && !q.VerificationTime.HasValue
                )
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}