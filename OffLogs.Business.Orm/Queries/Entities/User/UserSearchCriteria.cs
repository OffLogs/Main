using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserSearchCriteria: ICriterion
    {
        public UserStatus? Status { get; }
        public int Page { get; } = 1;

        public string Search { get; }
        public long[] ExceludeIds { get; }
        
        public UserSearchCriteria(string search, long[] exceludeIds = null)
        {
            Search = search;
            ExceludeIds = exceludeIds;
            if (Search == null)
                throw new ArgumentNullException(nameof(Search));
        }

        public UserSearchCriteria(UserStatus status, int page = 1)
        {
            Status = status;
            Page = page;
        }
    }
}
