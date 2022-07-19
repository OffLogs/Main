using Domain.Abstractions;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Monetization;

public interface IPaymentService: IDomainService
{
    PaymentPackageType GetActivePackageType(UserEntity user);
}
