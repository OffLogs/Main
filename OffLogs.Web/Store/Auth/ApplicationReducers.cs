using Fluxor;
using OffLogs.Web.Store.Auth.Actions;

namespace OffLogs.Web.Store.Auth;

public class AuthReducers
{
    [ReducerMethod(typeof(LogoutAction))]
    public static AuthState ReduceLogoutActionAction(AuthState state)
    {
        return new AuthState(null, null);
    }
    
    [ReducerMethod]
    public static AuthState ReduceLoginActionAction(AuthState state, LoginAction action)
    {
        return new AuthState(action.Jwt, action.Pem);
    }
}
