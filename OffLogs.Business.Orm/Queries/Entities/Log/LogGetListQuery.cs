using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Criteria.Entites;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetListQuery : LinqAsyncQueryBase<LogEntity, LogGetListCriteria, ListDto<LogEntity>>
    {
        public LogGetListQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<ListDto<LogEntity>> AskAsync(
            LogGetListCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            var pageSize = GlobalConstants.ListPageSize;
            var page = criterion.Page - 1;
            var offset = (int)((page <= 0 ? 0 : page) * pageSize);
            
            var logs = await TransactionProvider.CurrentSession.GetNamedQuery("Log.getList")
                .SetParameter("applicationId", criterion.ApplicationId)
                .SetParameter("logLevel", criterion.LogLevel)
                .SetFirstResult(offset)
                .SetMaxResults(pageSize)
                .ListAsync<LogEntity>(cancellationToken);
            var count = await TransactionProvider.CurrentSession.Query<LogEntity>()
                .Where(record => record.Application.Id == criterion.ApplicationId)
                .LongCountAsync(cancellationToken);
            
            return new ListDto<LogEntity>(logs, count);
        }
    }
}