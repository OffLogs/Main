using System.Collections.Generic;
using System.IO;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using Org.BouncyCastle.OpenSsl;

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
        using var textReader = new StringReader(pem);
        var pemReader = new PemReader(textReader);
    }
}
