using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Store.Notification.Effects;

public class FetchNotificationRulesEffect: Effect<FetchNotificationRulesAction>
{
    private readonly IApiService _apiService;
    private readonly ILogger<FetchNotificationRulesEffect> _logger;

    public FetchNotificationRulesEffect(
        IApiService apiService,
        ILogger<FetchNotificationRulesEffect> logger
    )
    {
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchNotificationRulesAction pageAction, IDispatcher dispatcher)
    {
        ListDto<NotificationRuleDto> response = null;
        try
        {
            response = await _apiService.NotificationRuleGetList();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
        dispatcher.Dispatch(new FetchNotificationRulesResultAction(response));
    }
}
