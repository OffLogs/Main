using System.Threading.Tasks;
using Fluxor;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Store.Common.Actions;
using OffLogs.Web.Store.Common.Effects;

namespace OffLogs.Web.Store.Auth.Effects;

public class LogoutEffect: AEffectPersistData<LogoutAction>
{
    public override Task HandleAsync(LogoutAction pageAction, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new PersistDataAction());
        return Task.CompletedTask;
    }
}
