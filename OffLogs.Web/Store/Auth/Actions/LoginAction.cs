namespace OffLogs.Web.Store.Auth.Actions;

public class LoginAction
{
    public string Pem { get; }
    
    public string PrivateKeyBase64 { get; set; }
    
    public string Jwt { get; }

    public LoginAction(string pem, string jwt, string privateKeyBase64)
    {
        Pem = pem;
        Jwt = jwt;
        PrivateKeyBase64 = privateKeyBase64;
    }
    
    public LoginAction(AuthState state)
    {
        Pem = state.Pem;
        Jwt = state.Jwt;
        PrivateKeyBase64 = state.PrivateKeyBase64;
    }
}
