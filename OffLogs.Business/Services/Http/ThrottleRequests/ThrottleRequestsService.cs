﻿using OffLogs.Business.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Http.ThrottleRequests
{
    /// <summary>
    /// The purpose of this service is validation maximum requests counter 
    /// for some entities(such as Applications)
    /// </summary>
    public class ThrottleRequestsService : IThrottleRequestsService
    {
        private ConcurrentBag<RequestItemModel> _items = new();

        private readonly TimeSpan _defaultCountingPeriond = TimeSpan.FromMinutes(1);

        public Task<int> CheckOrThowExceptionAsync(RequestItemType type, long itemId, int maxCounter = 500)
        {
            return CheckOrThowExceptionAsync(type, itemId, _defaultCountingPeriond, maxCounter);
        }

        public Task<int> CheckOrThowExceptionAsync(RequestItemType type, long itemId, TimeSpan countingPeriod, int maxCounter = 500)
        {
            var item = FindOrCreate(type, itemId, maxCounter, countingPeriod);
            if (item.IsPeriodOver)
            {
                item.ResetCounter();
                return Task.FromResult(item.Counter);
            }
            if (item.IsCounterTooBig)
            {
                throw new TooManyRequestsException(item.ItemId.ToString());
            }
            item.Increase();

            return Task.FromResult(item.Counter);
        }

        public void Clean()
        {
            _items.Clear();
        }

        private RequestItemModel FindOrCreate(RequestItemType type, long itemId, int maxCounter, TimeSpan countingPeriod)
        {
            var item = _items.FirstOrDefault(i => i.ItemId == itemId && i.Type == type);
            if (item == null)
            {
                item = new RequestItemModel(RequestItemType.Application, itemId, maxCounter, countingPeriod);
                _items.Add(item);
            }
            return item;
        }
    }
}
