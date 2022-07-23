using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Models;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Monetization;

public interface IPaymentPackageService: IDomainService
{
    PaymentPackageRestrictionsModel GetRestrictions(UserEntity user);

    Task ExtendOrChangePackage(UserEntity user, PaymentPackageType type, decimal paidSum);
}
