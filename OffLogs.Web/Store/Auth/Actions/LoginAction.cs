namespace OffLogs.Web.Store.Auth.Actions;

public class LoginAction
{
    public string Pem { get; }

    public LoginAction(string pem)
    {
        Pem = pem;
    }
}
