using Fluxor;
using OffLogs.Web.Store.Common.Actions;

namespace OffLogs.Web.Store.Common;

public class CommonReducers
{
    [ReducerMethod(typeof(SetIsAppInitializedAction))]
    public static CommonState ReduceLogoutActionAction(CommonState state)
    {
        return new CommonState(true);
    }
}
