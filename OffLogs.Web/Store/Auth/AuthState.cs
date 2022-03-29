using System.Collections.Generic;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Auth;

[FeatureState]
public class AuthState
{
    public bool IsLoggedIn => !string.IsNullOrEmpty(Jwt);

    public string Pem { get; set; }
    
    public string Jwt { get; set; }
    
    public AuthState() { }
    
    public AuthState(string jwt, string pem)
    {
        Jwt = jwt;
        Pem = pem;
    }
}
