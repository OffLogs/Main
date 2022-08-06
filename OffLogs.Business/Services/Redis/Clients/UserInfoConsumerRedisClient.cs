using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries.Entities.User;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Redis.Clients;

public class UserInfoConsumerRedisClient: IUserInfoConsumerRedisClient
{
    private readonly string _keyPattern = "offlogs_user_package_type_{0}";
    
    private readonly IRedisClient _redisClient;
    private readonly IAsyncQueryBuilder _queryBuilder;

    public UserInfoConsumerRedisClient(
        IRedisClient redisClient,
        IAsyncQueryBuilder queryBuilder
    )
    {
        _redisClient = redisClient;
        _queryBuilder = queryBuilder;
    }
    
    public async Task<PaymentPackageType?> GetUsersPaymentPackageType(long userId)
    {
        var packageTypeString = await _redisClient.GetString(GetPaymentPackageKey(userId));
        if (string.IsNullOrEmpty(packageTypeString))
        {
            return null;
        }

        if (Enum.TryParse<PaymentPackageType>(packageTypeString, out var parsedType))
        {
            return parsedType;
        }

        return null;
    }

    private string GetPaymentPackageKey(long userId)
    {
        return string.Format(_keyPattern, userId);
    }
}
