using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Log.Actions;

namespace OffLogs.Web.Store.Log.Effects;

public class FetchLogsEffect: Effect<FetchNextListPageAction>
{
    private readonly IState<LogsListState> _state;
    private readonly IState<AuthState> _authState;
    private readonly IApiService _apiService;
    private readonly ILogger<FetchLogsEffect> _logger;

    public FetchLogsEffect(
        IState<LogsListState> state,
        IState<AuthState> authState,
        IApiService apiService,
        ILogger<FetchLogsEffect> logger
    )
    {
        _state = state;
        _authState = authState;
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchNextListPageAction pageAction, IDispatcher dispatcher)
    {
        try
        {
            var response = await _apiService.GetLogsAsync(new GetListRequest
            {
                Page = _state.Value.Page,
                ApplicationId = _state.Value.ApplicationId,
                LogLevel = _state.Value.LogLevel,
                PrivateKeyBase64 = _authState.Value.PrivateKeyBase64
            });
            dispatcher.Dispatch(new FetchListResultAction(response));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
