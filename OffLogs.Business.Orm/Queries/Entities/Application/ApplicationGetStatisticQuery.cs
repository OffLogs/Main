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
using NHibernate.Criterion;
using NHibernate.Transform;
using OffLogs.Business.Orm.Dto.Entities;

namespace OffLogs.Business.Orm.Queries.Entities.Application
{
    public class ApplicationGetStatisticQuery : LinqAsyncQueryBase<FindByIdCriteria, ApplicationStatisticDto>
    {
        public ApplicationGetStatisticQuery(IDbSessionProvider transactionProvider)
            : base(transactionProvider)
        {
        }

        public override async Task<ApplicationStatisticDto> AskAsync(
            FindByIdCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            var logsCountQuery = QueryOver.Of<LogEntity>()
                .Where(entity => entity.Application.Id == criterion.Id)
                .ToRowCountQuery();
            
            var session = TransactionProvider.CurrentSession;

            ApplicationStatisticDto statisticDtoAlias = null;
            var resultList = await session.QueryOver<ApplicationStatisticDto>(() => statisticDtoAlias)
                .SelectList(projections =>
                {
                    projections.SelectSubQuery(logsCountQuery)
                        .WithAlias(() => statisticDtoAlias.LogsCount);
                    return projections;
                })
                .TransformUsing(Transformers.AliasToBean<ApplicationStatisticDto>())
                .ListAsync(cancellationToken);
            
            return resultList.FirstOrDefault();
        }
    }
}
