using System;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Store.Notification.Effects;

public class FetchMessageTemplatesEffect: Effect<FetchMessageTemplatesAction>
{
    private readonly IApiService _apiService;
    private readonly ILogger<FetchMessageTemplatesEffect> _logger;
    private readonly IState<NotificationRuleState> _state;

    public FetchMessageTemplatesEffect(
        IApiService apiService,
        ILogger<FetchMessageTemplatesEffect> logger,
        IState<NotificationRuleState> state
    )
    {
        _apiService = apiService;
        _logger = logger;
        _state = state;
    }

    public override async Task HandleAsync(FetchMessageTemplatesAction pageAction, IDispatcher dispatcher)
    {
        if (pageAction.IsLoadIfEmpty && _state.Value.MessageTemplates.Any())
        {
            return;
        }

        ListDto<MessageTemplateDto> response = null;
        try
        {
            response = await _apiService.MessageTemplateGetList();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
        finally
        {
            dispatcher.Dispatch(new FetchMessageTemplatesResultAction(response));    
        }
    }
}
