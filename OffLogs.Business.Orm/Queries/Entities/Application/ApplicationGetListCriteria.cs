using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Orm.Queries.Entities.Application
{
    public class ApplicationGetListCriteria: ICriterion
    {
        public long UserId { get; }
        public int Page { get; }

        public ApplicationGetListCriteria(long userId, int page)
        {
            UserId = userId;
            Page = page;
        }
    }
}
