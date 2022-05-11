using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;

namespace OffLogs.Web.Store.Log.Effects;

public class FetchLogEffect: Effect<FetchLogAction>
{
    private readonly IState<LogsListState> _state;
    private readonly IState<AuthState> _authState;
    private readonly IApiService _apiService;
    private readonly ILogger<FetchLogEffect> _logger;
    private readonly ToastService _toastService;

    public FetchLogEffect(
        IState<LogsListState> state,
        IState<AuthState> authState,
        IApiService apiService,
        ILogger<FetchLogEffect> logger,
        ToastService toastService
    )
    {
        _state = state;
        _authState = authState;
        _apiService = apiService;
        _logger = logger;
        _toastService = toastService;
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
            _toastService.AddInfoMessage(CommonResources.Error_LogNotFound);
            _logger.LogError(e.Message, e);
        }
    }
}
