using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace OffLogs.Business.Common.Encryption
{
    class PasswordFinder : IPasswordFinder
    {
        private string password;

        public PasswordFinder(string password)
        {
            this.password = password;
        }


        public char[] GetPassword()
        {
            return password.ToCharArray();
        }
    }
    
    public partial class AsymmetricEncryptor
    {
        public static AsymmetricEncryptor ReadFromPem(string pem, string password)
        {
            using var textReader = new StringReader(pem);
            var pemReader = new PemReader(textReader, new PasswordFinder(password));
            var privateKeyObject = pemReader.ReadObject();
            var rsaPrivateKey = (RsaPrivateCrtKeyParameters)privateKeyObject;
            var rsaPublicKey = new RsaKeyParameters(false, rsaPrivateKey.Modulus, rsaPrivateKey.PublicExponent);
            var keyPair = new AsymmetricCipherKeyPair(rsaPublicKey, rsaPrivateKey);
            return new AsymmetricEncryptor(keyPair);
        }
        
        public string CreatePem(string password)
        {
            var generator = new Pkcs8Generator(
                PrivateKey, 
                Pkcs8Generator.PbeSha1_3DES
            );
            generator.IterationCount = 4;
            generator.Password = password.ToCharArray();
            var pem = generator.Generate();

            using TextWriter textWriter = new StringWriter();
            var pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(pem);
            pemWriter.WriteObject(PublicKey);
            pemWriter.Writer.Flush();
            return textWriter.ToString();
        }
    }
}