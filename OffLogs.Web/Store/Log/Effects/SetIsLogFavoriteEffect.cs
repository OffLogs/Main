using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Store.Log.Effects;

public class SetIsLogFavoriteEffect: Effect<SetIsLogFavoriteAction>
{
    private readonly IState<LogsListState> _state;
    private readonly IState<AuthState> _authState;
    private readonly IApiService _apiService;
    private readonly ILogger<SetIsLogFavoriteEffect> _logger;
    private readonly ToastService _toastService;

    public SetIsLogFavoriteEffect(
        IState<LogsListState> state,
        IState<AuthState> authState,
        IApiService apiService,
        ILogger<SetIsLogFavoriteEffect> logger,
        ToastService toastService
    )
    {
        _state = state;
        _authState = authState;
        _apiService = apiService;
        _logger = logger;
        _toastService = toastService;
    }

    public override async Task HandleAsync(SetIsLogFavoriteAction pageAction, IDispatcher dispatcher)
    {
        try
        {
            await _apiService.LogSetIsFavoriteAsync(pageAction.LogId, pageAction.IsFavorite);
            if (pageAction.IsFavorite)
            {
                _toastService.AddInfoMessage("The log marked as favorite");    
            }
        }
        catch (Exception e)
        {
            _toastService.AddInfoMessage(CommonResources.Error_LogNotFound);
            _logger.LogError(e.Message, e);
        }
    }
}
