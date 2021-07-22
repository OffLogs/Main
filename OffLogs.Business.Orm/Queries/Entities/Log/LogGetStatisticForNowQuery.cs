using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Dto.Entities;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Transform;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetStatisticForNowQuery : LinqAsyncQueryBase<LogGetStatisticForNowCriteria, ICollection<LogStatisticForNowDto>>
    {
        public LogGetStatisticForNowQuery(IDbSessionProvider transactionProvider)
            : base(transactionProvider)
        {
        }

        public override async Task<ICollection<LogStatisticForNowDto>> AskAsync(
            LogGetStatisticForNowCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            return await TransactionProvider.CurrentSession
                .GetNamedQuery("Log.getStatisticForNow")
                .SetParameter("applicationId", criterion.ApplicationId)
                .SetResultTransformer(Transformers.AliasToBean<LogStatisticForNowDto>())
                .ListAsync<LogStatisticForNowDto>();
        }
    }
}