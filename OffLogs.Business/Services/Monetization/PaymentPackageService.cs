using System;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Models;
using OffLogs.Business.Orm.Entities.User;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Services.Monetization;

public class PaymentPackagePackageService: IPaymentPackageService
{
    private readonly IDbSessionProvider _dbSessionProvider;

    public PaymentPackagePackageService(IDbSessionProvider dbSessionProvider)
    {
        _dbSessionProvider = dbSessionProvider;
    }

    public async Task ExtendOrChangePackage(UserEntity user, PaymentPackageType type, decimal paidSum)
    {
        if (paidSum <= 0) throw new ArgumentException(nameof(paidSum));
        if (type == PaymentPackageType.Basic)
        {
            throw new Exception("The package can not be changed to Basic");
        }

        var newPackage = new UserPaymentPackageEntity()
        {
            Type = type,
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow,
        };
        
        // Calculate sum which left from previous package
        var lastPackage = user.LastPaymentPackage;
        if (lastPackage is { IsExpired: false })
        {
            var leftSum = lastPackage.LeftPaidDays * lastPackage.Type.GetPackageCostPerDay();
            paidSum += leftSum;
            lastPackage.ExpirationDate = DateTime.UtcNow.Date;
        }
        
        // Calculate paid period
        var paidDays = (int)Math.Ceiling(paidSum / type.GetPackageCostPerDay());
        newPackage.ExpirationDate = DateTime.UtcNow.Date.AddDays(paidDays);
        user.AddPaymentPackage(newPackage);
        await _dbSessionProvider.CurrentSession.UpdateAsync(user);
    }
    
    public PaymentPackageRestrictionsModel GetRestrictions(UserEntity user)
    {
        return user.ActivePaymentPackageType.GetRestrictions();
    }
}
