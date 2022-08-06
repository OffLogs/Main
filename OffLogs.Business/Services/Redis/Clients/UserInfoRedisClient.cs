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

public class UserInfoRedisClient: IUserInfoRedisClient
{
    private readonly string _keyPattern = "offlogs_user_package_type_{0}";
    
    private readonly IRedisClient _redisClient;
    private readonly IAsyncQueryBuilder _queryBuilder;

    public UserInfoRedisClient(
        IRedisClient redisClient,
        IAsyncQueryBuilder queryBuilder
    )
    {
        _redisClient = redisClient;
        _queryBuilder = queryBuilder;
    }

    public async Task SeedUsersPackages(CancellationToken cancellationToken = default)
    {
        int listPage = 1;
        ICollection<UserEntity> users = new List<UserEntity>();
        do
        {
            users = await _queryBuilder.For<ICollection<UserEntity>>()
                .WithAsync(new UserSearchCriteria(UserStatus.Active, listPage), cancellationToken);
            listPage++;
            
            foreach (var user in users)
            {
                var activePackage = user.ActivePaymentPackageType;
                var key = GetPaymentPackageKey(user.Id);
                await _redisClient.SetValueAsync(
                    key,
                    activePackage
                );    
            }
        } while (users.Count > 0);
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
