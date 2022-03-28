using Fluxor;

namespace OffLogs.Web.Store.Common.Effects;

public abstract class EffectPersistData<TTriggerAction>: Effect<TTriggerAction>
{
    protected const string AuthDataKey = "Store_AuthData";
}
