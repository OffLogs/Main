using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Auth;

[FeatureState]
public class AuthState
{
    public bool IsLoggedIn { get; }
    
    public string Pem { get; }
    
    public AuthState() { }
    
    public AuthState(bool isLoggedIn, string pem)
    {
        IsLoggedIn = isLoggedIn;
        Pem = pem;
    }
}
