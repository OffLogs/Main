using System.Text;
using OffLogs.Business.Common.Encryption;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Xunit;
using AsymmetricEncryptor = OffLogs.Business.Common.Security.AsymmetricEncryptor;

namespace OffLogs.Business.Common.Tests.Unit.Encryption
{
    public class PemParserTests
    {
        private string expectedPublicKey = @"MIIE9TAnBgoqhkiG9w0BDAEDMBkEFEqH6Nk5BGnpv9vdwKL5t5RbRlCkAgEEBIIE
yI76w4C/yTlBRFxzWJWLIfF9VOnUFv5UZaYThxp30Z/rOQ5DQ+xToOFYXvDfLIj7
EiGn9TpyaZjuCdqUJjMuYTGrRSGApLhBNEvEHYRLrKDsdd4+2vSsLCdv15bx72jE
VxbJfntPCZoJDcwgGF29iw1w4kQyjIH4GbuVi4HUrCExIxlUIhl1ieRwYJnUVzb0
Aj1/yY+Gw88uAQWvavg6hwbxdI+T+8YDQlB0FsxAOp1svpupb2mFNzPrc5bOYvD2
3abEPEnUSXuIjGSs08lu/RYILorKmdHMeaBdwaA1tD8j7uggIGVJgB4L9oT0Vq6V
1QjjObiLlbY+8IkzxD43bJsf6Rp0FPcceeosa8uTgwdspKuYTqsykYAKajd+FL83
/XmDzJ0Pu82ScF9LvVfCFnUEL7TO4mP04Im2fZJPyfJ4ZC6dq5x61CVoh50tQBF1
fvS3aQMjJoD0/vz1d4H4pzQBDU4Cjn6YYV1e7GG7ZPwMfhcqHv9oUD36+aCf67eI
wCdBkpuPxrlNask/14ikNwKSLgPctt4U5SRrZ8JixXnP0pZlq/THHDf+EYX1iC4G
Cna8sA1vO73slD3ji5E+SZ0on9OLILuH/CnImRUSc4ucb1B0Kd5+JAz4RMf0G0K3
MH7cbAXN5yStw16M3oo6HNwe01fZ4fcZc1MkCZpkrrGNhRtyVfRZOia3VDmpjY1y
doObpbiwbQ+P/LBmHavh2wzANVzmhywAPC5Qg2mgCmmlGpV4inxzN3s3S41bzumI
s6vCdcaKv9Z00ya0IU427Uzv8993ovFDRnKK5M9oiasF0hnl/KS78zVKlTxlATkh
F/OlMLQqSrD8kIU6Q4LbWGm5JPFJqWhkspp2h1nsuFBXbEGzI1KE5afMyqg1NX+2
pW1rytW/qykbjrbaOQqFVGN0RV9PmtBvW8ZpOWLgaQ/8oWNV8V7JFnH6pwA1yv0P
a8YINRXLsmnsQmq3MzaQQAlsDBJ8l47YCbDUQyjgGJjShjesfzJayTbEiGIHTkXR
xrKqOjIkb7XLH8WM6al37Cwc3fPWRJTVosQY8jqsqMjzs3i75qeF9J9RZTEtifs7
kvJpFtGLr5B/JXLMHngRoHf+3D5PyEbzSvRRw8iJNWenzt9kEJdNVUKBMYIl6KmX
r4Bp+W7P+P1Fam6fFBo2W0Fri56mOrFATDtLWqcv3CDRExPD+e6yFJ7MmxPrxs0/
ihsrLkezYggbVmFv5f9HEFoAf1NXIkvAoLYzXmu+md7I1A7p4OSCDQONfL4/FTxV
zF509sXdaoD4bCdHXocALMRoUvT2ym8b8Z0VqmcjNCkITJMbv2QnLATVLdgMDN2u
Zl52529bbCDfGUChUIsGXOtEnPD8BjWAkz0c2axLOgQnP//3WBP9XDj76yKuJQJ/
e3P5lnTOkG6r57tBMLG5aN05daE+Q3sbn6RZu8YLM82Cqfh6+dXGL8QWVzosEnaP
62v32Npm6Eg0NcM6HE69Ap3VNiO++In4b88nZbOACYCsJAeqLLXDps4B8Q1/9tQY
MUbw8LmMLosHKrPu3qL/cKCHxRp2Sn+zQaPfXQZJ0VIltiFP/aEpuopszaAs8360
Tw8Ht944GJDf3U2FPd1jxgpbEQSHzAN1ig==";
        private string expectedPrivateKey = @"MIIE9TAnBgoqhkiG9w0BDAEDMBkEFEqH6Nk5BGnpv9vdwKL5t5RbRlCkAgEEBIIE
yI76w4C/yTlBRFxzWJWLIfF9VOnUFv5UZaYThxp30Z/rOQ5DQ+xToOFYXvDfLIj7
EiGn9TpyaZjuCdqUJjMuYTGrRSGApLhBNEvEHYRLrKDsdd4+2vSsLCdv15bx72jE
VxbJfntPCZoJDcwgGF29iw1w4kQyjIH4GbuVi4HUrCExIxlUIhl1ieRwYJnUVzb0
Aj1/yY+Gw88uAQWvavg6hwbxdI+T+8YDQlB0FsxAOp1svpupb2mFNzPrc5bOYvD2
3abEPEnUSXuIjGSs08lu/RYILorKmdHMeaBdwaA1tD8j7uggIGVJgB4L9oT0Vq6V
1QjjObiLlbY+8IkzxD43bJsf6Rp0FPcceeosa8uTgwdspKuYTqsykYAKajd+FL83
/XmDzJ0Pu82ScF9LvVfCFnUEL7TO4mP04Im2fZJPyfJ4ZC6dq5x61CVoh50tQBF1
fvS3aQMjJoD0/vz1d4H4pzQBDU4Cjn6YYV1e7GG7ZPwMfhcqHv9oUD36+aCf67eI
wCdBkpuPxrlNask/14ikNwKSLgPctt4U5SRrZ8JixXnP0pZlq/THHDf+EYX1iC4G
Cna8sA1vO73slD3ji5E+SZ0on9OLILuH/CnImRUSc4ucb1B0Kd5+JAz4RMf0G0K3
MH7cbAXN5yStw16M3oo6HNwe01fZ4fcZc1MkCZpkrrGNhRtyVfRZOia3VDmpjY1y
doObpbiwbQ+P/LBmHavh2wzANVzmhywAPC5Qg2mgCmmlGpV4inxzN3s3S41bzumI
s6vCdcaKv9Z00ya0IU427Uzv8993ovFDRnKK5M9oiasF0hnl/KS78zVKlTxlATkh
F/OlMLQqSrD8kIU6Q4LbWGm5JPFJqWhkspp2h1nsuFBXbEGzI1KE5afMyqg1NX+2
pW1rytW/qykbjrbaOQqFVGN0RV9PmtBvW8ZpOWLgaQ/8oWNV8V7JFnH6pwA1yv0P
a8YINRXLsmnsQmq3MzaQQAlsDBJ8l47YCbDUQyjgGJjShjesfzJayTbEiGIHTkXR
xrKqOjIkb7XLH8WM6al37Cwc3fPWRJTVosQY8jqsqMjzs3i75qeF9J9RZTEtifs7
kvJpFtGLr5B/JXLMHngRoHf+3D5PyEbzSvRRw8iJNWenzt9kEJdNVUKBMYIl6KmX
r4Bp+W7P+P1Fam6fFBo2W0Fri56mOrFATDtLWqcv3CDRExPD+e6yFJ7MmxPrxs0/
ihsrLkezYggbVmFv5f9HEFoAf1NXIkvAoLYzXmu+md7I1A7p4OSCDQONfL4/FTxV
zF509sXdaoD4bCdHXocALMRoUvT2ym8b8Z0VqmcjNCkITJMbv2QnLATVLdgMDN2u
Zl52529bbCDfGUChUIsGXOtEnPD8BjWAkz0c2axLOgQnP//3WBP9XDj76yKuJQJ/
e3P5lnTOkG6r57tBMLG5aN05daE+Q3sbn6RZu8YLM82Cqfh6+dXGL8QWVzosEnaP
62v32Npm6Eg0NcM6HE69Ap3VNiO++In4b88nZbOACYCsJAeqLLXDps4B8Q1/9tQY
MUbw8LmMLosHKrPu3qL/cKCHxRp2Sn+zQaPfXQZJ0VIltiFP/aEpuopszaAs8360
Tw8Ht944GJDf3U2FPd1jxgpbEQSHzAN1ig==";
        
        public PemParserTests()
        {
        }

        [Fact]
        public void ShouldParsePemFileWithoutRemovingNewLines()
        {
            var pem = @$"
-----BEGIN ENCRYPTED PRIVATE KEY-----
{expectedPrivateKey}
-----END ENCRYPTED PRIVATE KEY-----
-----BEGIN PUBLIC KEY-----
{expectedPublicKey}
-----END PUBLIC KEY-----
";
            
            var parser = new PemParser(pem, false);
            Assert.Equal(expectedPrivateKey, parser.PrivateKeyBase64);
            Assert.Equal(expectedPublicKey, parser.PublicKeyBase64);
        }
        
        [Fact]
        public void ShouldParsePemFile()
        {
            var pem = @$"
-----BEGIN ENCRYPTED PRIVATE KEY-----
{expectedPrivateKey}
-----END ENCRYPTED PRIVATE KEY-----
-----BEGIN PUBLIC KEY-----
{expectedPublicKey}
-----END PUBLIC KEY-----
";
            
            var parser = new PemParser(pem, true);
            Assert.Equal(expectedPrivateKey.RemoveNewLines(), parser.PrivateKeyBase64);
            Assert.Equal(expectedPublicKey.RemoveNewLines(), parser.PublicKeyBase64);
        }
        
        [Fact]
        public void ShouldParseWithoutPublicKey()
        {
            var pem = @$"
-----BEGIN ENCRYPTED PRIVATE KEY-----
{expectedPrivateKey}
-----END ENCRYPTED PRIVATE KEY-----
";
            
            var parser = new PemParser(pem, false);
            Assert.Equal(expectedPrivateKey, parser.PrivateKeyBase64);
            Assert.Empty(parser.PublicKeyBase64);
        }
        
        [Fact]
        public void ShouldParseWithoutPrivateKey()
        {
            var pem = @$"
-----BEGIN PUBLIC KEY-----
{expectedPublicKey}
-----END PUBLIC KEY-----
";
            
            var parser = new PemParser(pem, false);
            Assert.Equal(expectedPublicKey, parser.PublicKeyBase64);
            Assert.Empty(parser.PrivateKeyBase64);
        }
    }
}
