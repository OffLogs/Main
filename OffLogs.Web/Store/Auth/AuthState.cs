using System.Collections.Generic;
using System.IO;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Security;
using Org.BouncyCastle.OpenSsl;

namespace OffLogs.Web.Store.Auth;

[FeatureState]
public class AuthState
{
    public bool IsLoggedIn => !string.IsNullOrEmpty(Jwt);

    public string Pem { get; set; }
    
    public string Jwt { get; set; }
    
    public string PrivateKeyBase64 { get; set; }

    private PemParser _pemParser;
    
    public AuthState() { }
    
    public AuthState(string jwt, string pem, string privateKeyBase64)
    {
        Jwt = jwt;
        Pem = pem;
        PrivateKeyBase64 = privateKeyBase64;
    }

    private void InitPemParser()
    {
        if (_pemParser != null)
        {
            return;
        }

        _pemParser = new PemParser(Pem);
    }
}
