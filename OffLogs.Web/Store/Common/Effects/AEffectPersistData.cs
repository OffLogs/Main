using Fluxor;

namespace OffLogs.Web.Store.Common.Effects;

public abstract class AEffectPersistData<TTriggerAction>: Effect<TTriggerAction>
{
    protected const string AuthDataKey = "Store_AuthData";
}
