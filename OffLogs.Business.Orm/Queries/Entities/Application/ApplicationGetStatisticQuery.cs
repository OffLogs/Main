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
            var commonLogsQuery = QueryOver.Of<LogEntity>()
                .Where(entity => entity.Application.Id == criterion.Id);
            
            var errorLogsCountQuery = commonLogsQuery.Clone()
                .And(entity => entity.Level == LogLevel.Error)
                .ToRowCountQuery();
            
            var debugLogsCountQuery = commonLogsQuery.Clone()
                .And(entity => entity.Level == LogLevel.Debug)
                .ToRowCountQuery();
            
            var fatalLogsCountQuery = commonLogsQuery.Clone()
                .And(entity => entity.Level == LogLevel.Fatal)
                .ToRowCountQuery();
            
            var infoLogsCountQuery = commonLogsQuery.Clone()
                .And(entity => entity.Level == LogLevel.Information)
                .ToRowCountQuery();
            
            var warningLogsCountQuery = commonLogsQuery.Clone()
                .And(entity => entity.Level == LogLevel.Warning)
                .ToRowCountQuery();
            
            var tracesCountQuery = QueryOver.Of<LogTraceEntity>()
                .JoinQueryOver(property => property.Log)
                    .Where(entity => entity.Application.Id == criterion.Id)
                .ToRowCountQuery();
            
            var propertiesCountQuery = QueryOver.Of<LogPropertyEntity>()
                .JoinQueryOver(property => property.Log)
                    .Where(entity => entity.Application.Id == criterion.Id)
                .ToRowCountQuery();
            
            var session = TransactionProvider.CurrentSession;

            LogEntity aliasLog = null;
            ApplicationStatisticDto aliasStatistic = null;
            var resultList = await session.QueryOver<LogEntity>(() => aliasLog)
                .SelectList(projections =>
                {
                    projections.SelectCount(entity => entity.Id).WithAlias(() => aliasStatistic.LogsCount);
                    projections.SelectSubQuery(errorLogsCountQuery).WithAlias(() => aliasStatistic.ErrorLogsCount);
                    projections.SelectSubQuery(debugLogsCountQuery).WithAlias(() => aliasStatistic.DebugLogsCount);
                    projections.SelectSubQuery(fatalLogsCountQuery).WithAlias(() => aliasStatistic.FatalLogsCount);
                    projections.SelectSubQuery(infoLogsCountQuery).WithAlias(() => aliasStatistic.InformationLogsCount);
                    projections.SelectSubQuery(warningLogsCountQuery).WithAlias(() => aliasStatistic.WarningLogsCount);
                    projections.SelectSubQuery(tracesCountQuery).WithAlias(() => aliasStatistic.TracesCount);
                    projections.SelectSubQuery(propertiesCountQuery).WithAlias(() => aliasStatistic.PropertiesCount);
                    
                    return projections;
                })
                .Where(() => aliasLog.Application.Id == criterion.Id)
                .TransformUsing(Transformers.AliasToBean<ApplicationStatisticDto>())
                .ListAsync<ApplicationStatisticDto>(cancellationToken);
            
            return resultList.FirstOrDefault();
        }
    }
}
