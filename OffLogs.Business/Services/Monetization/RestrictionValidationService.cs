using System.Threading.Tasks;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Monetization;

public class RestrictionValidationService: IRestrictionValidationService
{
    public RestrictionValidationService()
    {
        
    }

    public void CheckNotificationRulesAddingAvailable(UserEntity user)
    {
        var maxNotificationRulesCounter = user.ActivePaymentPackageType
            .GetRestrictions()
            .MaxNotificationRules;
        if (maxNotificationRulesCounter == 0)
        {
            return;
        }

        if (user.NotificationRules.Count >= maxNotificationRulesCounter)
        {
            throw new PaymentPackageRestrictionException();
        }
    }
}
