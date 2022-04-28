using System;
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
    public class LogGetStatisticForNowQuery : LinqAsyncQueryBase<GetByApplicationOrUserCriteria, ICollection<LogStatisticForNowDto>>
    {
        public LogGetStatisticForNowQuery(IDbSessionProvider transactionProvider)
            : base(transactionProvider)
        {
        }

        public override async Task<ICollection<LogStatisticForNowDto>> AskAsync(
            GetByApplicationOrUserCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            return await TransactionProvider.CurrentSession
                .GetNamedQuery("Log.getStatisticForNow")
                .SetParameter("applicationId", criterion.ApplicationId)
                .SetParameter("userId", criterion.UserId)
                .SetParameter("dateNow", DateTime.UtcNow)
                .SetResultTransformer(Transformers.AliasToBean<LogStatisticForNowDto>())
                .ListAsync<LogStatisticForNowDto>(cancellationToken);
        }
    }
}
