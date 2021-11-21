using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Http.ThrottleRequests
{
    public interface IThrottleRequestsService
    {
        Task<int> CheckOrThrowExceptionAsync(RequestItemType type, long itemId, int maxCounter = 500);

        Task<int> CheckOrThrowExceptionAsync(RequestItemType type, long itemId, TimeSpan countingPeriod, int maxCounter = 500);
        void Clean();
    }
}
