using System.Text.RegularExpressions;
using OffLogs.Business.Common.Extensions;

namespace OffLogs.Business.Common.Security;

public class PemParser
{
    private static readonly Regex PrivateKeyRegex = new(@"(-----BEGIN[\s\w]+PRIVATE KEY-----(?<privateKey>(\n|\r|\r\n)([0-9a-zA-Z\+\/=]{64}(\n|\r|\r\n))*([0-9a-zA-Z\+\/=]{1,63}(\n|\r|\r\n))?)-----END[\s\w]+PRIVATE KEY-----)");
    private static readonly Regex PublicKeyRegex = new(@"(-----BEGIN PUBLIC KEY-----(?<publicKey>(\n|\r|\r\n)([0-9a-zA-Z\+\/=]{64}(\n|\r|\r\n))*([0-9a-zA-Z\+\/=]{1,63}(\n|\r|\r\n))?)-----END PUBLIC KEY-----)");
    
    public string PrivateKeyBase64 { get; }
    
    public string PublicKeyBase64 { get; }

    public PemParser(string pem, bool isRemoveNewLines = true)
    {
        if (string.IsNullOrEmpty(pem))
        {
            return;
        }

        var matches = PrivateKeyRegex.Match(pem);
        PrivateKeyBase64 = matches.Groups["privateKey"].Value.Trim();
        PrivateKeyBase64 = isRemoveNewLines ? PrivateKeyBase64.RemoveNewLines() : PrivateKeyBase64;
        
        matches = PublicKeyRegex.Match(pem);
        PublicKeyBase64 = matches.Groups["publicKey"].Value.Trim();
        PublicKeyBase64 = isRemoveNewLines ? PublicKeyBase64.RemoveNewLines() : PublicKeyBase64;
    }
}
