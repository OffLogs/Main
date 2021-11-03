using System.Text;
using OffLogs.Business.Common.Encryption;
using Org.BouncyCastle.Crypto;
using Xunit;

namespace OffLogs.Business.Common.Tests.Unit.Encryption
{
    public class RsaEncryptionServiceTests
    {
        private AsymmetricEncryptor _encryptor;

        public RsaEncryptionServiceTests()
        {
            _encryptor = AsymmetricEncryptor.GenerateKeyPair();
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
        public void ShouldThrowExceptionIfDecryptionDataLengthIsIncorrect()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            Assert.Throws<InvalidCipherTextException>(() =>
            {
                _encryptor.DecryptData(new byte[]{ 1, 2 });
            });
        }
        
        [Fact]
        public void ShouldThrowExceptionIfDecryptionDataIsIncorrect()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            Assert.Throws<InvalidCipherTextException>(() =>
            {
                var encryptor2 = AsymmetricEncryptor.GenerateKeyPair();
                encryptor2.DecryptData(encrypted);
            });
        }
        
        [Fact]
        public void ShouldNotDecryptData()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            Assert.Throws<InvalidCipherTextException>(() =>
            {
                _encryptor.DecryptData(new byte[]{ 1, 2 });
            });
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
        
        [Fact]
        public void SignShouldNotBeCorrect()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var sign = _encryptor.SignData(dataBytes);
            
            var encryptor2 = AsymmetricEncryptor.GenerateKeyPair();
            var isValidSign = encryptor2.VerifySign(dataBytes, sign);
            Assert.False(isValidSign);
        }
    }
}