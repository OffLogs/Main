using System;
using System.Linq;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Orm.Entities.User;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Monetization;

public class PaymentService: IPaymentService
{
    private readonly IAsyncQueryBuilder _queryBuilder;

    public PaymentService(IAsyncQueryBuilder queryBuilder)
    {
        _queryBuilder = queryBuilder;
    }

    public PaymentPackageType GetActivePackageType(UserEntity user)
    {
        var lastPackage = user.PaymentPackages
            .MaxBy(item => item.CreateTime);
        if (lastPackage == null || lastPackage.IsExpired)
        {
            return PaymentPackageType.Basic;
        }

        return lastPackage.Type;
    }
}
