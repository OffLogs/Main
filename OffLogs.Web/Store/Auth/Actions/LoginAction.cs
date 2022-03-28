namespace OffLogs.Web.Store.Auth.Actions;

public class LoginAction
{
    public string Pem { get; }
    
    public string Jwt { get; }

    public LoginAction(string pem, string jwt)
    {
        Pem = pem;
        Jwt = jwt;
    }
}
