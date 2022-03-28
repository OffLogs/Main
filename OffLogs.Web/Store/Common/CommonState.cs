using Fluxor;

namespace OffLogs.Web.Store.Common;

[FeatureState]
public class CommonState
{
    public readonly bool IsInitialized;

    public CommonState() { }
    
    public CommonState(bool isInitialized)
    {
        IsInitialized = isInitialized;
    }
}
