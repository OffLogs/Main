using System;
using OffLogs.Business.Common.Exceptions.Common;
using OffLogs.Business.Common.Models;

namespace OffLogs.Business.Common.Constants.Monetization;

public enum PaymentPackageType
{
    Basic = 1,
    Standart,
    Pro
}

public static class PaymentPackageTypeExtensions
{
    public static PaymentPackageRestrictionsModel GetRestrictions(this PaymentPackageType level)
    {
        switch (level)
        {
            case PaymentPackageType.Basic:
                return new PaymentPackageRestrictionsModel
                {
                    MaxApiRequests = 50,
                    MaxNotificationRules = 5,
                    MaxUserEmails = 3,
                    MinNotificationRuleTimeout = 60 * 60 * 6, // 6 hours
                    LogsExpirationTimeout = 60 * 60 * 24 * 7, // 1 week
                    FavoriteLogsExpirationTimeout = 60 * 60 * 24 * 31 * 3, // 3 months
                    MaxApplications = 5
                };
            case PaymentPackageType.Standart:
                return new PaymentPackageRestrictionsModel
                {
                    MaxApiRequests = 300,
                    MaxNotificationRules = 20,
                    MaxUserEmails = 10,
                    MinNotificationRuleTimeout = 60 * 30, // 30 minutes
                    LogsExpirationTimeout = 60 * 60 * 24 * 31 * 1, // 1 month
                    FavoriteLogsExpirationTimeout = 60 * 60 * 24 * 31 * 6, // 6 months
                    MaxApplications = 15
                };
            case PaymentPackageType.Pro:
                return new PaymentPackageRestrictionsModel
                {
                    MaxApiRequests = 600,
                    MaxNotificationRules = 0, // unlimited
                    MaxUserEmails = 50,
                    MinNotificationRuleTimeout = 60 * 5, // 5 minutes
                    LogsExpirationTimeout = 60 * 60 * 24 * 31 * 6, // 6 month
                    FavoriteLogsExpirationTimeout = 0, // always
                    MaxApplications = 45
                };
        }
        throw new ItemNotFoundException();
    }
    
    public static decimal GetPackageCostPerMonth(this PaymentPackageType level)
    {
        switch (level)
        {
            case PaymentPackageType.Basic:
                return 0.0m;
            case PaymentPackageType.Standart:
                return 10.0m;
            case PaymentPackageType.Pro:
                return 30.0m;
        }
        throw new ItemNotFoundException();
    }
    
    public static decimal GetPackageCostPerDay(this PaymentPackageType level)
    {
        return level.GetPackageCostPerMonth() / 31;
    }
}
