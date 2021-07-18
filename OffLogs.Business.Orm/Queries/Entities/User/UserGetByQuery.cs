using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserGetByQuery : LinqAsyncQueryBase<UserEntity, UserGetByCriteria, UserEntity>
    {
        public UserGetByQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<UserEntity> AskAsync(UserGetByCriteria criterion, CancellationToken cancellationToken = default)
        {
            return await TransactionProvider.CurrentSession.Query<UserEntity>()
                .Where(q => q.UserName == criterion.UserName)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}