﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Connection;
using OffLogs.Business.Orm.Criteria.Entites;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Orm.Queries.Entities
{
    public class LogGetListQuery<THasId> : LinqAsyncQueryBase<THasId, LogGetListCriteria, ListDto<LogEntity>>
        where THasId : class, IHasId, new()
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