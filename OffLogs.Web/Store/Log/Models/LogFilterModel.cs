using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Store.Log.Models;

public record struct LogFilterModel(
    long ApplicationId,
    bool IsOnlyFavorite = false,
    string Search = null,
    LogLevel? LogLevel = null
);
