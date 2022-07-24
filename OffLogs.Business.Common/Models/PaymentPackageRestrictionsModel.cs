namespace OffLogs.Business.Common.Models;

public record struct PaymentPackageRestrictionsModel(
    int MaxApiRequests,
    int MaxNotificationRules,
    int MaxUserEmails,
    int MinNotificationRuleTimeout,
    int LogsExpirationTimeout,
    int FavoriteLogsExpirationTimeout,
    int MaxApplications
);
