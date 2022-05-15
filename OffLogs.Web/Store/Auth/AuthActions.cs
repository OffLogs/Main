using System.Collections.Generic;
using System.IO;
using Fluxor;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Security;
using Org.BouncyCastle.OpenSsl;

namespace OffLogs.Web.Store.Auth;

public record struct LogoutAction();

public record struct LoginAction(string Pem, string Jwt, string PrivateKeyBase64)
{
    public LoginAction(AuthState state) : this(state.Pem, state.Jwt, state.PrivateKeyBase64)
    {
    }
}
