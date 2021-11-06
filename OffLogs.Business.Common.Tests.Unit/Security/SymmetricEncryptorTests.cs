using System.Text;
using OffLogs.Business.Common.Encryption;
using OffLogs.Business.Common.Security;
using Org.BouncyCastle.Crypto;
using Xunit;

namespace OffLogs.Business.Common.Tests.Unit.Encryption
{
    public class SymmetricEncryptorTests
    {
        private SymmetricEncryptor _encryptor;

        public SymmetricEncryptorTests()
        {
            _encryptor = SymmetricEncryptor.GenerateKey();
        }

        [Fact]
        public void ShouldCreateKeyPair()
        {
            Assert.NotNull(_encryptor.Key);
            Assert.True(_encryptor.Key.GetKey().Length == 32);
        }
        
        [Fact]
        public void ShouldEncryptData()
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
        public void ShouldDecryptStringData()
        {
            var dataString = "some data";
            var encrypted = _encryptor.EncryptData(dataString);

            var decrypted = _encryptor.DecryptData(encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted);
            Assert.Equal(decryptedString, dataString);
        }
    }
}