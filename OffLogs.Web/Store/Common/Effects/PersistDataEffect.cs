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
    private readonly IState<AuthState> _authState;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<PersistDataAEffect> _logger;

    public PersistDataAEffect(
        IState<AuthState> authState,
        ILocalStorageService localStorage,
        ILogger<PersistDataAEffect> logger
    )
    {
        _authState = authState;
        _localStorage = localStorage;
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
