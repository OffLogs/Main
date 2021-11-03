using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using OffLogs.Business.Common.Encryption;
using OffLogs.Business.Common.Extensions;
using Xunit;

namespace OffLogs.Business.Common.Tests.Unit.Encryption.RsaEncryption
{
    public class RsaEncryptionServiceTests
    {
        private RsaEncryptor _encryptor;

        public RsaEncryptionServiceTests()
        {
            _encryptor = RsaEncryptor.GenerateKeyPair();
        }

        [Fact]
        public void ShouldCreateKeyPair()
        {
            Assert.NotNull(_encryptor.PrivateKey);
            Assert.NotNull(_encryptor.PrivateKey);
        }
        
        [Fact]
        public void ShouldEncryptDataWithPublicKey()
        {
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var encrypted = _encryptor.EncryptData(data);
            Assert.NotNull(encrypted);
            Assert.True(encrypted.Length > 0);
        }
        
        [Fact]
        public void ShouldDecryptData()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            var decrypted = _encryptor.DecryptData(encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted);
            Assert.Equal(decryptedString, dataString);
        }
        
        [Fact]
        public void ShouldSignDataAndValidateThisSign()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var sign = _encryptor.SignData(dataBytes);
            var isValidSign = _encryptor.VerifySign(dataBytes, sign);
            Assert.True(isValidSign);
        }
    }
}