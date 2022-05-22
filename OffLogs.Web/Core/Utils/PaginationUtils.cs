using System;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Core.Utils;

public static class PaginationUtils
{
    public static int CalculatePage(int skip, int pageSize = GlobalConstants.ListPageSize)
    {
        var page = (int) Math.Ceiling((decimal) (skip / pageSize));
        return page == 0 ? 1 : page + 1;
    }
}
