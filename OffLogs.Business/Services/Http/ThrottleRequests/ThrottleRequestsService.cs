using OffLogs.Business.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Http.ThrottleRequests
{
    public class ThrottleRequestsService : IThrottleRequestsService
    {
        private ConcurrentBag<RequestItemModel> _items = new ConcurrentBag<RequestItemModel>();

        private readonly TimeSpan _countingPeriond = TimeSpan.FromMinutes(1);

        public Task CheckOrThowException(long itemId, int maxCounter = 500)
        {
            var item = FindOrCreate(itemId, maxCounter);
            if (DateTime.Now - item.CountStartTime > _countingPeriond)
            {
                item.ResetCounter();
                return Task.CompletedTask;
            }
            if (item.IsCounterTooBig)
            {
                throw new TooManyRequestsException(item.ItemId.ToString());
            }
            item.Increase();

            return Task.CompletedTask;
        }

        private RequestItemModel FindOrCreate(long itemId, int maxCounter)
        {
            var item = _items.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null)
            {
                item = new RequestItemModel(maxCounter);
                _items.Add(item);
            }
            return item;
        }
    }
}
