using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace OffLogs.Business.Common.Security
{
    public partial class AsymmetricEncryptor
    {
        private static readonly int RsaKeyLength = 2048;
        
        private const string Algorithm = "RSA/ECB/OAEPWithSHA256AndMGF1Padding";
        private const string SignatureAlgorithm = "SHA512WITHRSA";
        private const int DefaultRsaBlockSize = 190;

        public RsaPrivateCrtKeyParameters PrivateKey { get; }
        public RsaKeyParameters PublicKey { get; }

        #region Initialization
        
        public AsymmetricEncryptor(AsymmetricCipherKeyPair pair) : this(pair.Public, pair.Private)
        {
        }

        public AsymmetricEncryptor(AsymmetricKeyParameter publicKey, AsymmetricKeyParameter privateKey = null)
        {
            if (privateKey != null)
            {
                PrivateKey = (RsaPrivateCrtKeyParameters)privateKey;    
            }
            PublicKey = (RsaKeyParameters)publicKey;
        }
        
        public static Security.AsymmetricEncryptor GenerateKeyPair()
        {
            var randomGenerator = new SecureRandom();
            var keyParameters = new KeyGenerationParameters(randomGenerator, RsaKeyLength);
            //Initialize the key constructor with parameters
            var keyGenerator = new RsaKeyPairGenerator();
            keyGenerator.Init(keyParameters);
            var keyPair = keyGenerator.GenerateKeyPair();
            //Get public and private keys
            var publicKey = keyPair.Public;
            if (((RsaKeyParameters)publicKey).Modulus.BitLength < RsaKeyLength)
            {
                throw new Exception($"Failed key generation. Key length {RsaKeyLength}");
            }
            return new Security.AsymmetricEncryptor(keyPair);
        }
        
        public static Security.AsymmetricEncryptor FromPrivateKeyBytes(byte[] privateKeyBytes)
        {
            var privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKeyBytes);
            var publicKey = new RsaKeyParameters(false, privateKey.Modulus, privateKey.PublicExponent);
            return new Security.AsymmetricEncryptor(publicKey, privateKey);
        }
        
        public static Security.AsymmetricEncryptor FromPublicKeyBytes(byte[] publicKey)
        {
            return new Security.AsymmetricEncryptor(
                PublicKeyFactory.CreateKey(publicKey)
            );
        }
        
        #endregion

        public byte[] EncryptData(string data)
        {
            return EncryptData(
                Encoding.UTF8.GetBytes(data)
            );
        }

        public byte[] EncryptData(byte[] data)
        {
            if (PublicKey == null) throw new ArgumentNullException(nameof(PublicKey));
         
            var cipher = CipherUtilities.GetCipher(Algorithm);
            cipher.Init(true, PublicKey);
            return ApplyCipher(data, cipher, DefaultRsaBlockSize);
        }
        
        public byte[] DecryptData(byte[] data)
        {
            if (PrivateKey == null) throw new ArgumentNullException(nameof(PrivateKey));
            var cipher = CipherUtilities.GetCipher(Algorithm);
            cipher.Init(false, PrivateKey);
            
            int blockSize = PrivateKey.Modulus.BitLength / 8;
            return ApplyCipher(data, cipher, blockSize);
        }
        
        public byte[] SignData(byte[] data)
        {
            if (PrivateKey == null) throw new ArgumentNullException(nameof(PrivateKey));
 
            var signer = SignerUtilities.GetSigner(SignatureAlgorithm);
            signer.Init(true, PrivateKey);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }
        
        public bool VerifySign(byte[] data, byte[] sign)
        {
            if (PublicKey == null) throw new ArgumentNullException(nameof(PublicKey));

            var signer = SignerUtilities.GetSigner(SignatureAlgorithm);
            signer.Init(false, PublicKey);
            signer.BlockUpdate(data, 0, data.Length);

            return signer.VerifySignature(sign);
        }

        public bool IsValidPublicKey(byte[] publicKey)
        {
            try
            {
                return PublicKeyFactory.CreateKey(publicKey) != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        private byte[] ApplyCipher(byte[] data, IBufferedCipher cipher, int blockSize)
        {
            var inputStream = new MemoryStream(data);
            var outputBytes = new List<byte>();

            int index;
            var buffer = new byte[blockSize];
            while ((index = inputStream.Read(buffer, 0, blockSize)) > 0)
            {
                var cipherBlock = cipher.DoFinal(buffer, 0, index);
                outputBytes.AddRange(cipherBlock);
            }

            return outputBytes.ToArray();
        }

        #region Byte Conversion

        public byte[] GetPrivateKeyBytes()
        {
            if (PrivateKey == null) throw new ArgumentNullException(nameof(PrivateKey));

            var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(PrivateKey);
            return privateKeyInfo.GetEncoded();
        }
        
        public byte[] GetPublicKeyBytes()
        {
            var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(PublicKey);
            return publicKeyInfo.GetEncoded();
        }

        #endregion
    }
}