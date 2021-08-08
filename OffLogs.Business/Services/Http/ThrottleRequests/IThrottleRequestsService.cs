using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Http.ThrottleRequests
{
    public interface IThrottleRequestsService: IDomainService
    {
        Task<int> CheckOrThowExceptionAsync(long itemId, int maxCounter = 500);

        Task<int> CheckOrThowExceptionAsync(long itemId, TimeSpan countingPeriod, int maxCounter = 500);
    }
}
