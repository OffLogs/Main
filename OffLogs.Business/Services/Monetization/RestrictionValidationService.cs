using System;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Monetization;

public class RestrictionValidationService: IRestrictionValidationService
{
    public void CheckNotificationRulesAddingAvailable(NotificationRuleEntity newRule)
    {
        if (newRule.User == null)
        {
            throw new ArgumentException(nameof(newRule.User));
        }

        var restrictions = newRule.User.ActivePaymentPackageType.GetRestrictions();
        var maxRuleTimeout = restrictions.MaxNotificationRuleTimeout;
        if (newRule.Period < maxRuleTimeout)
        {
            throw new PaymentPackageRestrictionException();
        }

        var maxItemsCounter = restrictions.MaxNotificationRules;
        if (maxItemsCounter == 0)
        {
            return;
        }

        if (newRule.User.NotificationRules.Count >= maxItemsCounter)
        {
            throw new PaymentPackageRestrictionException();
        }
    }
    
    public void CheckUserEmailsAddingAvailable(UserEntity user)
    {
        var maxItemsCounter = user.ActivePaymentPackageType
            .GetRestrictions()
            .MaxUserEmails;
        if (maxItemsCounter == 0)
        {
            return;
        }

        if (user.Emails.Count >= maxItemsCounter)
        {
            throw new PaymentPackageRestrictionException();
        }
    }
    
    public void CheckApplicationsAddingAvailable(UserEntity user)
    {
        var maxItemsCounter = user.ActivePaymentPackageType
            .GetRestrictions()
            .MaxApplications;

        if (user.Applications.Count >= maxItemsCounter)
        {
            throw new PaymentPackageRestrictionException();
        }
    }
}
