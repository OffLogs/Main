using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Common.Actions;

namespace OffLogs.Web.Store.Common.Effects;

public class PersistDataAEffect: AEffectPersistData<PersistDataAction>
{
    private readonly IState<CommonState> _authState;
    private readonly ILocalStorageService _localStorage;
    private readonly IDispatcher _dispatcher;
    private readonly ILogger<PersistDataAEffect> _logger;

    public PersistDataAEffect(
        IState<CommonState> authState,
        ILocalStorageService localStorage,
        IDispatcher dispatcher,
        ILogger<PersistDataAEffect> logger
    )
    {
        _authState = authState;
        _localStorage = localStorage;
        _dispatcher = dispatcher;
        _logger = logger;
    }

    public override async Task HandleAsync(PersistDataAction pageAction, IDispatcher dispatcher)
    {
        _logger.LogDebug("Persist data to local storage");
        await SetData(AuthDataKey, _authState.Value);
    }

    private async Task SetData(string key, object data)
    {
        await _localStorage.SetItemAsStringAsync(
            key,
            JsonConvert.SerializeObject(data)
        );
    }
}
