using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries.Criterias;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.UserEmail
{
    public class UserEmailGetByTokenQuery : LinqAsyncQueryBase<UserEmailEntity, GetByTokenCriteria, UserEmailEntity>
    {
        public UserEmailGetByTokenQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<UserEmailEntity> AskAsync(GetByTokenCriteria criterion, CancellationToken cancellationToken = default)
        {
            return await TransactionProvider.CurrentSession.Query<UserEmailEntity>()
                .Where(q => q.VerificationToken == criterion.Token)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
