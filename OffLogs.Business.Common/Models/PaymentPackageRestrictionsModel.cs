namespace OffLogs.Business.Common.Models;

public record struct PaymentPackageRestrictionsModel(
    int MaxApiRequests,
    int MaxNotificationRules,
    int MaxUserEmails,
    int MaxNotificationRuleTimeout,
    int LogsExpirationTimeout,
    int FavoriteLogsExpirationTimeout
);
