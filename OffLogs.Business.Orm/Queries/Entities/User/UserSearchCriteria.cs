using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserSearchCriteria: ICriterion
    {
        public UserSearchCriteria(string search, long[] exceludeIds = null)
        {
            Search = search;
            ExceludeIds = exceludeIds;
            if (Search == null)
                throw new ArgumentNullException(nameof(Search));
        }

        public string Search { get; }
        public long[] ExceludeIds { get; }
    }
}
