using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Store.Log.Effects;

public class FetchLogEffect: Effect<FetchLogAction>
{
    private readonly IState<LogsListState> _state;
    private readonly IState<AuthState> _authState;
    private readonly IApiService _apiService;
    private readonly ILogger<FetchLogEffect> _logger;

    public FetchLogEffect(
        IState<LogsListState> state,
        IState<AuthState> authState,
        IApiService apiService,
        ILogger<FetchLogEffect> logger
    )
    {
        _state = state;
        _authState = authState;
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchLogAction pageAction, IDispatcher dispatcher)
    {
        try
        {
            var response = await _apiService.GetLogAsync(pageAction.LogId, _authState.Value.PrivateKeyBase64);
            dispatcher.Dispatch(new AddLogDetailsAction(response));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
