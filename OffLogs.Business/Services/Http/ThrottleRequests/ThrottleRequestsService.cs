using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Exceptions.Common;
using OffLogs.Business.Services.Redis.Clients;

namespace OffLogs.Business.Services.Http.ThrottleRequests
{
    /// <summary>
    /// The purpose of this service is validation maximum requests counter 
    /// for some entities(such as Applications)
    /// </summary>
    public class ThrottleRequestsService : IThrottleRequestsService
    {
        private readonly IUserInfoRedisClient _userInfoRedisClient;
        private ConcurrentBag<RequestItemModel> _items = new();

        private readonly TimeSpan _defaultCountingPeriod = TimeSpan.FromMinutes(1);

        public bool _isEnabled { get; }
        
        public ThrottleRequestsService(
            IConfiguration configuration,
            IUserInfoRedisClient userInfoRedisClient
        )
        {
            _userInfoRedisClient = userInfoRedisClient;
            _isEnabled = configuration.GetValue<bool>("App:IsThrottleTooManyRequests", true);
        }

        public async Task<int> CheckOrThrowExceptionByApplicationIdAsync(long applicationId, long userId)
        {
            var usersPackageType = await _userInfoRedisClient.GetUsersPaymentPackageType(userId);
            if (!usersPackageType.HasValue)
            {
                usersPackageType = PaymentPackageType.Basic;
            }
            return await CheckOrThrowExceptionAsync(
                RequestItemType.Application,
                applicationId,
                _defaultCountingPeriod,
                usersPackageType.Value.GetRestrictions().MaxApiRequests
            );
        }

        public Task<int> CheckOrThrowExceptionAsync(RequestItemType type, long itemId, TimeSpan countingPeriod, int maxCounter = 500)
        {
            if (!_isEnabled)
            {
                return Task.FromResult(0);
            }

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
