using System;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace OffLogs.Business.Common.Encryption
{
    public class SymmetricEncryptor
    {
        private readonly string Algorithm = "AES256";
        private readonly string CypherAlgorithm = "AES/CTR/NoPadding";
        private readonly byte[] DefaultIV = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private readonly IBufferedCipher _decryptor;
        private readonly IBufferedCipher _encryptor;
        
        public KeyParameter Key { get; }
        public byte[] IV { get; }
        
        private static readonly CipherKeyGenerator _keyGenerator;
        
        static SymmetricEncryptor()
        {
            _keyGenerator = GeneratorUtilities.GetKeyGenerator("AES256");
        }

        public SymmetricEncryptor(byte[] key, byte[] iv = null)
        {
            Key = ParameterUtilities.CreateKeyParameter(Algorithm, key);
            IV = iv ?? DefaultIV;
            var parametersWithIv = new ParametersWithIV(Key, IV);
            
            _encryptor = CipherUtilities.GetCipher(CypherAlgorithm);
            _encryptor.Init(true, parametersWithIv);
            
            _decryptor = CipherUtilities.GetCipher(CypherAlgorithm);
            _decryptor.Init(false, parametersWithIv);
        }
        
        public static SymmetricEncryptor GenerateKey()
        {
            var key = _keyGenerator.GenerateKey();
            return new SymmetricEncryptor(key);
        }
        
        public byte[] EncryptData(byte[] data)
        {
            if (Key == null) throw new ArgumentNullException(nameof(Key));
            
            return _encryptor.DoFinal(data);
        }
        
        public byte[] DecryptData(byte[] data)
        {
            if (Key == null) throw new ArgumentNullException(nameof(Key));
            
            return _decryptor.DoFinal(data);
        }
    }
}